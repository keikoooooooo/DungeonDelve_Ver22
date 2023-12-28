using System;
using System.Threading.Tasks;
using Cinemachine;
using FMOD.Studio;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerDataHandle))]
public abstract class PlayerStateMachine : MonoBehaviour, IDamageable
{
    #region Variables
    #region Ref
    [field: Header("BaseClass -------")] // REF
    [field: SerializeField] public PlayerInputs input { get; private set; }
    [field: SerializeField] public Transform model { get; private set; }
    [field: SerializeField] public Animator animator { get; private set; }
    [field: SerializeField] public CharacterController characterController { get; private set; }
    [field: SerializeField] public CinemachineFreeLook cinemachineFreeLook { get; private set; }
    [field: SerializeField] public ParticleSystem enableEffect { get; private set; }
    [field: SerializeField] public PlayerDataHandle playerData { get; private set; }
    [field: SerializeField] public PlayerVoice voice { get; private set; }
    [field: Header("BaseClass/Set Material")]
    [field: SerializeField] public M_SetEmission setEmission { get; private set; }
    [field: SerializeField] public M_SetFloat setDissolve { get; private set; }
    #endregion

    #region Get & Set Property
    public SO_PlayerConfiguration PlayerConfig => playerData.PlayerConfig;
    public StatusHandle Health { get; private set; }
    public StatusHandle Stamina { get; private set; }
    public PlayerBaseState CurrentState { get; set; }
    public Vector3 AppliedMovement { get; set; }
    public Vector3 InputMovement { get; set; }
    public bool IsGrounded => characterController.isGrounded;
    protected bool CanMove { get; set; }
    protected bool CanRotation { get; set; }
    protected bool CanAttack { get; set; }
    public bool IsIdle => input.Move.magnitude == 0;
    protected bool IsJump => input.Space && !input.LeftShift && !animator.IsTag("Damage", 1);
    public bool IsWalk => !IsIdle && _movementState == MovementState.StateWalk;
    public bool IsRun => !IsIdle && IsGrounded && !input.LeftShift && _movementState == MovementState.StateRun;
    public bool IsDash => input.LeftShift && IsGrounded && Stamina.CurrentValue >= PlayerConfig.GetDashSTCost();
    public bool CanIncreaseST { get; set; } // có thể tăng ST không ?
    public bool CanFootstepsAudioPlay { get; set; }
    #endregion
    
    #region Animation IDs Paramater
    // FLOAT
    [HideInInspector] public int IDSpeed = Animator.StringToHash("Speed");                  //  idle, walk, run, 
    [HideInInspector] public int IDHorizontal = Animator.StringToHash("Horizontal");        //  trục x: Left & Right
    [HideInInspector] public int IDVertical = Animator.StringToHash("Vertical");            //  trục z: Fornt & Back
    [HideInInspector] public int IDStateTime = Animator.StringToHash("StateTime");          //  thời gian phát animation đã trôi qua
    // BOOL
    [HideInInspector] public int IDJump = Animator.StringToHash("Jump");                    //  nhảy
    [HideInInspector] public int IDFall = Animator.StringToHash("Fall");                    //  rơi
    [HideInInspector] public int IDWeaponEquip = Animator.StringToHash("WeaponEquipped ");  //  switchMode: cầm / không cầm vũ khí
    [HideInInspector] public int ID4Direction = Animator.StringToHash("4Direction");        //  di chuyển 4 hướng: LFRB
    [HideInInspector] public int IDDead = Animator.StringToHash("Dead");                    //  die
    // TRIGGER
    [HideInInspector] public int IDDamageFall = Animator.StringToHash("Damage_Fall");       //  nhận sát thương (ngã)
    [HideInInspector] public int IDDamageStand = Animator.StringToHash("Damage_Stand");     //  nhận sát thương (đứng)
    [HideInInspector] public int IDDash = Animator.StringToHash("Dash");                    //  lướt
    [HideInInspector] public int IDNormalAttack = Animator.StringToHash("NormalAttack");    //  tấn công
    [HideInInspector] public int IDChargedAttack = Animator.StringToHash("ChargedAttack");  //  tấn công
    [HideInInspector] public int IDSkill = Animator.StringToHash("Skill");                  //  kỹ năng
    [HideInInspector] public int IDSpecial = Animator.StringToHash("Special");              //  kỹ năng đặc biệt
    #endregion
    
    #region Events
    public event Action OnDashEvent;
    public event Action OnTakeDMGEvent;
    public event Action OnDieEvent;
    #endregion

    #region Variables Config
    private PlayerStateFactory _state;
    protected enum MovementState
    {
        StateWalk,
        StateRun
    }
    [HideInInspector] protected MovementState _movementState;
    [HideInInspector] protected Camera _mainCamera;
    [HideInInspector] private float _gravity;
    [HideInInspector] private bool _pressedJump;
    [HideInInspector] private float _jumpVelocity;
    [HideInInspector] private float _jumpCD_Temp;
    [HideInInspector] private float _delayIncreaseST;
    [HideInInspector] protected int _attackCounter;
    [HideInInspector] protected float _skillCD_Temp;
    [HideInInspector] protected float _burstCD_Temp;
    [HideInInspector] private int _currentHP;
    [HideInInspector] private PLAYBACK_STATE _footstepsPLAYBACK_STATE;
    [HideInInspector] public EventInstance _footstepsInstance;
    public EventInstance walkFootsteps { get; private set; }
    public EventInstance runFootsteps { get; private set; }
    public EventInstance runfastFootsteps { get; private set; }
    #endregion
    #endregion

    private void Awake()
    {
        GetReference();
    }
    private void OnEnable()
    {
        SetVariables();
        HandleEnable();
        DamageableData.Add(gameObject, this);
    }
    private void Start()
    {
        CreateAudioRef();
    }
    protected virtual void Update()
    {
        HandleInput();
        
        CurrentState.UpdateState();
        
        HandleJumping();
        
        HandleMovement();
        
        HandleRotation();

        HandleStamina();

        HandleFootstepsAudio();
    }
    private void OnDisable()
    {
        DamageableData.Remove(gameObject);
    }


    private void GetReference()
    {
        _mainCamera = Camera.main;
        _state = new PlayerStateFactory(this);
        Health = new StatusHandle();
        Stamina = new StatusHandle();
    }
    private void CreateAudioRef()
    {
        walkFootsteps = AudioManager.CreateInstance(FMOD_Events.Instance.walkFootsteps);
        runFootsteps = AudioManager.CreateInstance(FMOD_Events.Instance.runFootsteps);
        runfastFootsteps = AudioManager.CreateInstance(FMOD_Events.Instance.runfastFootsteps);
    }

    
    /// <summary> Khởi tạo giá trị biến ban đầu, và sẽ gọi hàm mỗi lần object này được OnEnable. </summary>
    protected virtual void SetVariables()
    { 
        // Set State
        CurrentState = _state.Idle();
        CurrentState.EnterState();
        
        // Set Config
        _gravity = -30f;
        CanMove = true;
        CanRotation = true;
        CanIncreaseST = true;
        _movementState = MovementState.StateRun;
        characterController.enabled = true;
        input.PlayerInput.Enable();
    }
    
    
    private void HandleEnable()
    {
        setDissolve.ChangeDurationApply(1f);
        setDissolve.ChangeCurrentValue(1f);
        setDissolve.ChangeValueSet(0f);
        setDissolve.Apply();
        enableEffect.gameObject.SetActive(true);
        enableEffect.Play();
    }
    private void HandleInput()
    {
        // Giá trị di chuyển
        var trans = transform;
        InputMovement = trans.right * input.Move.x + trans.forward * input.Move.y;
        InputMovement = Quaternion.AngleAxis(_mainCamera.transform.rotation.eulerAngles.y, Vector3.up) * InputMovement;
        
        // Thời gian hồi chiêu
        _skillCD_Temp = _skillCD_Temp > 0 ? _skillCD_Temp - Time.deltaTime : 0;
        _burstCD_Temp = _burstCD_Temp > 0 ? _burstCD_Temp - Time.deltaTime : 0;
        
        if (!input.ChangeState) return; // Chuyển mode: Walk <=> Run
        input.ChangeState = false;
        _movementState = _movementState == MovementState.StateRun ? MovementState.StateWalk : MovementState.StateRun;
    }
    private void HandleMovement()
    {
        if (!CanMove || !characterController.enabled) 
            return;
        
        characterController.Move(AppliedMovement * Time.deltaTime 
                                 + new Vector3(0f, _jumpVelocity, 0f) * Time.deltaTime);
    }
    private void HandleJumping()
    {
        if (IsGrounded && _jumpVelocity < 0)
            _jumpVelocity = -9.81f;
        
        switch (IsGrounded)
        {
            case true when _pressedJump && IsIdle:
                _pressedJump = false;
                animator.SetBool(IDFall, true);
                break;
            
            case true when _jumpCD_Temp > 0:
                _jumpCD_Temp -= Time.deltaTime;
                animator.SetBool(IDJump, false);
                break;
            
            case true when _jumpCD_Temp <= 0 && IsJump:
                input.LeftMouse = false;
                animator.SetBool(IDJump, true);
                animator.SetBool(IDFall, false);
                _jumpVelocity = Mathf.Sqrt(PlayerConfig.GetJumpHeight() * -2f * _gravity);
                voice.PlayJumping();
                ReleaseAction();
                break;
        }
        if (!IsGrounded)
        {
            _jumpCD_Temp = PlayerConfig.GetJumpCD();
            input.LeftMouse = false;
            input.Space = false;
            _pressedJump = true;
        }
        _jumpVelocity += _gravity * Time.deltaTime;
    }
    private void HandleRotation()
    {
        if (InputMovement == Vector3.zero || !CanRotation) 
            return;
        var rotation = Quaternion.LookRotation(InputMovement, Vector3.up);
        model.rotation = Quaternion.RotateTowards(model.rotation, rotation, 1000 * Time.deltaTime);
    }
    private void HandleStamina()
    {
        if(!CanIncreaseST || Stamina.CurrentValue >= Stamina.MaxValue) return;

        if (_delayIncreaseST > 0)
        {
            _delayIncreaseST -= Time.deltaTime;
            return;
        }

        _delayIncreaseST = .15f;
        Stamina.Increases(2);
    }
    private void HandleFootstepsAudio()
    {
        if (IsGrounded && !IsIdle && CanFootstepsAudioPlay)
        {
            _footstepsInstance.getPlaybackState(out _footstepsPLAYBACK_STATE);
            if (_footstepsPLAYBACK_STATE == PLAYBACK_STATE.STOPPED)
            {
                _footstepsInstance.start();
            }
        }
        else
        {
            _footstepsInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
    
    
    #region Handle Damageable
    public void CauseDMG(GameObject _gameObject, AttackType _attackType)
    {
        if (!DamageableData.Contains(_gameObject, out var iDamageable)) return;

        var _damage = _attackType switch
        {
            AttackType.NormalAttack => CalculationDMG(PercentDMG_NA()),
            AttackType.ChargedAttack => CalculationDMG(PercentDMG_CA()),
            AttackType.ElementalSkill => CalculationDMG(PercentDMG_ES()),
            AttackType.ElementalBurst => CalculationDMG(PercentDMG_EB()),
            _ => 1
        };

        var _isCrit = false;  // Có kích CRIT không ?
        if (Random.value <= PlayerConfig.GetCRITRate() / 100)
        {
            var critDMG = (PlayerConfig.GetCRITDMG() + 100.0f) / 100.0f; // vì là DMG cộng thêm nên cần phải +100%DMG vào
            _damage = Mathf.CeilToInt(_damage * critDMG);
            _isCrit = true;
        } 
        
        // Gọi takeDMG trên đối tượng vừa va chạm
        iDamageable.TakeDMG(_damage, _isCrit);
    }
    public void TakeDMG(int _damage, bool _isCRIT) 
    {
        // Nếu đòn đánh là CRIT thì sẽ nhận Random DEF từ giá trị 0 -> DEF ban đầu / 2, nếu không sẽ lấy 100% DEF ban đầu
        var _valueDef = _isCRIT ? Random.Range(0, PlayerConfig.GetDEF() * 0.5f) : PlayerConfig.GetDEF();
        
        // Tính lượng DMG thực nhận vào sau khi trừ đi lượng DEF
        var _finalDmg = (int)Mathf.Max(0, _damage - Mathf.Max(0, _valueDef));
        
        Health.Decreases(_finalDmg);
        DMGPopUpGenerator.Instance.Create(transform.position, _finalDmg, _isCRIT, false);
        
        if (Health.CurrentValue <= 0)
        {
            CurrentState.SwitchState(_state.Dead());
            CallbackDieEvent();
            return;
        }
        
        if (animator.IsTag("Damage", 1)) return;
        
        HandleDamage();
        SetPlayerInputState(false);
        InputMovement = Vector3.zero;
        AppliedMovement = Vector3.zero;
        input.Move =Vector3.zero;
        CurrentState.SwitchState(_isCRIT ? _state.DamageFall() : _state.DamageStand());
        CallbackTakeDMGEvent();
    }

    public virtual float PercentDMG_NA() => PlayerConfig.GetNormalAttackMultiplier()[_attackCounter].GetMultiplier()[PlayerConfig.GetWeaponLevel() - 1];
    public virtual float PercentDMG_CA() => PlayerConfig.GetChargedAttackMultiplier()[0].GetMultiplier()[PlayerConfig.GetWeaponLevel() - 1];
    public virtual float PercentDMG_ES() => PlayerConfig.GetElementalSkillMultiplier()[0].GetMultiplier()[PlayerConfig.GetWeaponLevel() - 1]; 
    public virtual float PercentDMG_EB() => PlayerConfig.GetElementalBurstMultiplier()[0].GetMultiplier()[PlayerConfig.GetWeaponLevel() - 1];
    public virtual int CalculationDMG(float _percent) => Mathf.CeilToInt(PlayerConfig.GetATK() * (_percent / 100.0f)); 
    public void ReleaseDamageState() // gọi trên animationEvent để giải phóng trạng thái TakeDamage
    {
        CurrentState.SwitchState(_state.Idle());
        ResetDamageTrigger();
        SetPlayerInputState(true);
    }
    public void SetPlayerInputState(bool _stateValue)
    {
        if (_stateValue)
        {
            input.PlayerInput.Player.Move.Enable();
            input.PlayerInput.Player.Jump.Enable();
            input.PlayerInput.Player.NormalAttack.Enable();
            input.PlayerInput.Player.ElementalSkill.Enable();
            input.PlayerInput.Player.ElementalBurst.Enable();
            return;
        }
        input.PlayerInput.Player.Move.Disable();
        input.PlayerInput.Player.Jump.Disable();
        input.PlayerInput.Player.NormalAttack.Disable();
        input.PlayerInput.Player.ElementalSkill.Disable();
        input.PlayerInput.Player.ElementalBurst.Disable();
    }
    public void ResetDamageTrigger()
    {
        animator.ResetTrigger(IDDamageFall);
        animator.ResetTrigger(IDDamageStand);
    }
    public async void ResetDeadState()
    {
        EnemyTracker.Clear();
        HandleEnable();
        transform.position = Vector3.zero;
        model.rotation = Quaternion.Euler(Vector3.zero);
        animator.SetBool(IDDead,false);
        setEmission.ChangeIntensitySet(0);
        setEmission.ChangeDurationApply(0);
        setEmission.Apply();
        cinemachineFreeLook.m_XAxis.Value = 0;
        cinemachineFreeLook.m_YAxis.Value = .5f;
        cinemachineFreeLook.enabled = true;
        characterController.enabled = true;

        var _hp = PlayerConfig.GetHP();
        var _st = PlayerConfig.GetST();
        Health.InitValue(_hp, _hp);
        Stamina.InitValue(_st, _st);
        await Task.Delay(700);
        ReleaseDamageState();
    }
    #endregion

    
    #region Event Callback
    public void CallbackDashEvent() => OnDashEvent?.Invoke();
    private void CallbackTakeDMGEvent() => OnTakeDMGEvent?.Invoke();
    private void CallbackDieEvent() => OnDieEvent?.Invoke();
    #endregion
    

    /// <summary> Khi nhận sát thương, nếu nhân vật cần thực hiện hành vi thì Override lại. </summary>
    protected virtual void HandleDamage() { }

    /// <summary> Giải phóng tất cả trạng thái khi nhảy, lướt,... </summary>
    public abstract void ReleaseAction();
}

