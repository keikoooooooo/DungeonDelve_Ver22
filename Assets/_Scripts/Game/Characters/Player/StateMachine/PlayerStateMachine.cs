using System;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class PlayerStateMachine : MonoBehaviour, IDamageable
{
    #region Variables
    [Header("BaseClass -------")]
    public PlayerInputs inputs;
    public Transform model;
    public Animator animator;
    public CharacterController characterController;
    public CinemachineFreeLook cinemachineFreeLook;
    public ParticleSystem enableEffect;
    
    #region Get & Set Property
    public PlayerDataHandle PlayerData;
    public SO_PlayerConfiguration PlayerConfig => PlayerData.PlayerConfig;
    public StatusHandle Health { get; private set; }
    public StatusHandle Stamina { get; private set; }
    public PlayerBaseState CurrentState { get; set; }
    public Vector3 AppliedMovement { get; set; }
    public Vector3 InputMovement { get; set; }
    
    public bool IsGrounded => characterController.isGrounded;
    protected bool CanMove { get; set; }
    protected bool CanRotation { get; set; }
    protected bool CanAttack { get; set; }
    public bool IsIdle => inputs.Move.magnitude == 0;
    protected bool IsJump => inputs.Space && !inputs.LeftShift && !animator.IsTag("Damage", 1);
    public bool IsWalk => !IsIdle && _movementState == MovementState.StateWalk;
    public bool IsRun => !IsIdle && IsGrounded && !inputs.LeftShift && _movementState == MovementState.StateRun;
    public bool IsDash => inputs.LeftShift && IsGrounded && Stamina.CurrentValue >= PlayerConfig.GetDashSTCost();

    public bool CanIncreaseST { get; set; } // có thể tăng ST không ?
    #endregion
    
    [Header("Set Material")] 
    public M_SetEmission setEmission;
    public M_SetFloat setDissolve;
    
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
    
    // Events
    public event Action E_Dash;
    
    // Player Config
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
    [HideInInspector] protected float _specialCD_Temp;
    [HideInInspector] private int _currentHP;
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
    protected virtual void Update()
    {
        HandleInput();
        
        CurrentState.UpdateState();
        
        HandleJumping();
        
        HandleMovement();
        
        HandleRotation();

        HandleStamina();
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

    /// <summary>
    /// Khởi tạo giá trị biến ban đầu, và sẽ gọi hàm mỗi lần object này được OnEnable
    /// </summary>
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
        inputs.PlayerInput.Enable();
        
        InitStatus();
    }
    public void InitStatus()
    {
        var _maxHP = PlayerConfig.GetHP();
        var _maxST = PlayerConfig.GetST();
        Health.InitValue(_maxHP);
        Stamina.InitValue(_maxST);

        // _currentHP = PlayerPrefs.GetInt(PlayerConfig.NameCode.ToString(), _maxHP);
        // Health.Decreases(_maxHP - _currentHP);
    }
    

    private void HandleEnable()
    {
        setDissolve.ChangeDurationApply(1f);
        setDissolve.ChangeCurrentValue(1f);
        setDissolve.ChangeValueSet(0f);
        setDissolve.Apply();
        enableEffect.Play();
    }
    private void HandleInput()
    {
        // Giá trị di chuyển
        var trans = transform;
        InputMovement = trans.right * inputs.Move.x + trans.forward * inputs.Move.y;
        InputMovement = Quaternion.AngleAxis(_mainCamera.transform.rotation.eulerAngles.y, Vector3.up) * InputMovement;
        
        // Thời gian hồi chiêu
        _skillCD_Temp = _skillCD_Temp > 0 ? _skillCD_Temp - Time.deltaTime : 0;
        _specialCD_Temp = _specialCD_Temp > 0 ? _specialCD_Temp - Time.deltaTime : 0;
        
        if (!inputs.ChangeState) return; // Chuyển mode: Walk <=> Run
        inputs.ChangeState = false;
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
                inputs.LeftMouse = false;
                animator.SetBool(IDJump, true);
                animator.SetBool(IDFall, false);
                _jumpVelocity = Mathf.Sqrt(PlayerConfig.GetJumpHeight() * -2f * _gravity);
                ReleaseAction();
                break;
        }
        if (!IsGrounded)
        {
            _jumpCD_Temp = PlayerConfig.GetJumpCD();
            inputs.LeftMouse = false;
            inputs.Space = false;
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
    
    
    #region Handle Damageable
    public void CauseDMG(GameObject _gameObject, AttackType _attackType)
    {
        if (!DamageableData.Contains(_gameObject, out var iDamageable)) return;

        var _damage = _attackType switch
        {
            AttackType.NormalAttack => CalculationDMG(PercentDMG_NA()),
            AttackType.ChargedAttack => CalculationDMG(PercentDMG_CA()),
            AttackType.ElementalSkill => CalculationDMG(PercentDMG_EK()),
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
        ReleaseAction();
        InputMovement = Vector3.zero;
        AppliedMovement = Vector3.zero;
        inputs.Move =Vector3.zero;
        inputs.PlayerInput.Disable();
        
        // Nếu đòn đánh là CRIT thì sẽ nhận Random DEF từ giá trị 0 -> DEF ban đầu / 2, nếu không sẽ lấy 100% DEF ban đầu
        var _valueDef = _isCRIT ? Random.Range(0, PlayerConfig.GetDEF() * 0.5f) : PlayerConfig.GetDEF();
        
        // Tính lượng DMG thực nhận vào sau khi trừ đi lượng DEF
        var _finalDmg = (int)Mathf.Max(0, _damage - Mathf.Max(0, _valueDef));
        
        Health.Decreases(_finalDmg);
        DMGPopUpGenerator.Instance.Create(transform.position, _finalDmg, _isCRIT, false);
        
        if (Health.CurrentValue <= 0)
        {
            CurrentState.SwitchState(_state.Dead());
            return;
        }
        
        HandleDamage();
        CurrentState.SwitchState(_isCRIT ? _state.DamageFall() : _state.DamageStand());
    }

    public virtual float PercentDMG_NA() => PlayerConfig.GetNormalAttackMultiplier()[_attackCounter].GetMultiplier()[PlayerConfig.GetWeaponLevel() - 1];
    public virtual float PercentDMG_CA() => PlayerConfig.GetChargedAttackMultiplier()[0].GetMultiplier()[PlayerConfig.GetWeaponLevel() - 1];
    public virtual float PercentDMG_EK() => PlayerConfig.GetElementalSkillMultiplier()[0].GetMultiplier()[PlayerConfig.GetWeaponLevel() - 1]; 
    public virtual float PercentDMG_EB() => PlayerConfig.GetElementalBurstMultiplier()[0].GetMultiplier()[PlayerConfig.GetWeaponLevel() - 1];
    public virtual int CalculationDMG(float _percent) => Mathf.CeilToInt(PlayerConfig.GetATK() * (_percent / 100.0f)); 
    
    public void ReleaseDamageState() // gọi trên animationEvent để giải phóng trạng thái TakeDamage
    {
        CurrentState.SwitchState(_state.Idle());
        inputs.PlayerInput.Enable();
    }
    #endregion

    
    #region Event Callback
    public void OnDashEvent() => E_Dash?.Invoke();
    #endregion
    

    /// <summary>
    /// Khi nhận sát thương, nếu nhân vật cần thực hiện hành vi thì Override lại
    /// </summary>
    protected virtual void HandleDamage() { }

    /// <summary>
    /// Giải phóng tất cả trạng thái khi nhảy, lướt,
    /// </summary>
    public abstract void ReleaseAction();

    // private void OnApplicationQuit()
    // {
    //     PlayerPrefs.SetInt(PlayerConfig.NameCode.ToString(), Health.CurrentValue);
    // }
}

