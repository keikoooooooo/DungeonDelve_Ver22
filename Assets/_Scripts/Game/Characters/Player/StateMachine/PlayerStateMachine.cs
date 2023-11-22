using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class PlayerStateMachine : MonoBehaviour, IDamageable, ICalculateDMG
{
    #region Variables
    [Header("BaseClass -------")]
    public PlayerInputs inputs;
    public Transform model;
    public Animator animator;
    public CharacterController characterController;
    
    #region Get & Set Property
    [field: SerializeField] public PlayerConfiguration PlayerConfig { get; private set; }
    public StatusHandle Health { get; private set; }
    public StatusHandle Stamina { get; private set; }
    public PlayerBaseState CurrentState { get; set; }
    public Vector3 AppliedMovement { get; set; }
    public Vector3 InputMovement { get; set; }
    
    protected bool IsGrounded => characterController.isGrounded;
    public bool CanControl { get; set; } // nhân vật có thể điều khiển ?
    protected bool CanMove { get; set; }
    protected bool CanRotation { get; set; }
    protected bool CanAttack { get; set; }
    public bool IsIdle => inputs.move.magnitude == 0;
    public bool IsJump => inputs.space && !inputs.leftShift && !animator.IsTag(1, "Damage");
    public bool IsWalk => !IsIdle && _movementState == MovementState.StateWalk;
    public bool IsRun => !IsIdle && IsGrounded && !inputs.leftShift && _movementState == MovementState.StateRun;
    public bool IsDash => inputs.leftShift && IsGrounded && Stamina.CurrentValue >= PlayerConfig.DashEnergy;

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
    public event Action<float> E_SkillCD; 
    public event Action<float> E_SpecialCD;
    
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
    [HideInInspector] protected float _skillCD_Temp;
    [HideInInspector] protected float _specialCD_Temp;
    [HideInInspector] protected int _calculatedDamage;
    
    // Coroutine
    private Coroutine _handleSTCoroutine;
    private Coroutine _handleDamageCoroutine;
    #endregion


    private void Awake()
    {
        GetReference();
    }
    private void OnEnable()
    {
        SetVariables();
    }
    protected virtual void Update()
    {
        HandleInput();
        
        CurrentState.UpdateState();
        
        HandleJumping();
        
        HandleMovement();
        
        HandleRotation();
    }
    private void OnDisable()
    {
        DamageableData.Remove(gameObject);
    }

    
    private void GetReference()
    {
        _mainCamera = Camera.main;
        _state = new PlayerStateFactory(this);
        characterController = GetComponent<CharacterController>();
        Health = new StatusHandle(PlayerConfig.MaxHealth);
        Stamina = new StatusHandle(PlayerConfig.MaxStamina);
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
        CanControl = true;
        CanMove = true;
        CanRotation = true;
        CanIncreaseST = true;
        _movementState = MovementState.StateRun;
        characterController.enabled = true;
        
        if (_handleSTCoroutine != null) StopCoroutine(_handleSTCoroutine);
        _handleSTCoroutine = StartCoroutine(IncreaseSTCoroutine());
        
        // Event
        DamageableData.Add(gameObject, this);
    }


    private void HandleInput()
    {
        // Giá trị di chuyển
        var trans = transform;
        InputMovement = trans.right * inputs.move.x + trans.forward * inputs.move.y;
        InputMovement = Quaternion.AngleAxis(_mainCamera.transform.rotation.eulerAngles.y, Vector3.up) * InputMovement;
        
        // Thời gian hồi chiêu
        _skillCD_Temp = _skillCD_Temp > 0 ? _skillCD_Temp - Time.deltaTime : 0;
        _specialCD_Temp = _specialCD_Temp > 0 ? _specialCD_Temp - Time.deltaTime : 0;
        
        if (!inputs.changeState) return; // Chuyển mode: Walk <=> Run
        inputs.changeState = false;
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
                inputs.leftMouse = false;
                animator.SetBool(IDJump, true);
                animator.SetBool(IDFall, false);
                _jumpVelocity = Mathf.Sqrt(PlayerConfig.JumpHeight * -2f * _gravity);
                ReleaseAction();
                break;
        }
        if (!IsGrounded)
        {
            _jumpCD_Temp = PlayerConfig.JumpCD;
            inputs.leftMouse = false;
            inputs.space = false;
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
    
    public IEnumerator IncreaseSTCoroutine()
    {
        while (true)
        {
            if (Stamina.CurrentValue <= 100 && CanIncreaseST)
            {
                Stamina.Increase(2);
            }
            yield return new WaitForSeconds(.15f);
        }
    } 
    
    
    #region HandleDMG
    public void CauseDMG(GameObject _gameObject)
    {
        if (!DamageableData.Contains(_gameObject, out var iDamageable)) return;
        
        // Có kích CRIT không ?
        var critRateRandom = Random.value;
        var _isCrit = false;
        if (critRateRandom <= PlayerConfig.CRITRate / 100)
        {
            var critDMG = (PlayerConfig.CRITDMG + 100.0f) / 100.0f; // vì là DMG cộng thêm nên cần phải +100%DMG vào
            _calculatedDamage = Mathf.CeilToInt(_calculatedDamage * critDMG);
            _isCrit = true;
        } 
        
        iDamageable.TakeDMG(_calculatedDamage, _isCrit);
    }
    public void TakeDMG(int _damage, bool _isCRIT) 
    {
        // Nếu đòn đánh là CRIT thì sẽ nhận Random DEF từ giá trị 0 -> DEF ban đầu / 2, nếu không sẽ lấy 100% DEF ban đầu
        var _valueDef = _isCRIT ? Random.Range(0, PlayerConfig.DEF * 0.5f) : PlayerConfig.DEF;
        
        // Tính lượng DMG thực nhận vào sau khi trừ đi lượng DEF
        var _def = Mathf.CeilToInt(_damage * (_valueDef / 100.0f));
        _damage -= _def;
        
        Health.Subtract(_damage);
        DMGPopUpGenerator.Instance.Create(transform.position, _damage, _isCRIT, false);


        if (Health.CurrentValue <= 0)
        {
            Die();
            return;
        }

        HandleDamage();
        
        if(_handleDamageCoroutine != null) StopCoroutine(_handleDamageCoroutine);
        if (_isCRIT)
        {
            animator.SetTrigger(IDDamageFall);
            _handleDamageCoroutine = StartCoroutine(HandleDamageCoroutine(.35f, 8));
        }
        else
        {
            animator.SetTrigger(IDDamageStand);
            _handleDamageCoroutine = StartCoroutine(HandleDamageCoroutine(.2f,1.5f));
        }
        
        CanMove = false;
        CanAttack = false;
        CanRotation = false;
    }

    private IEnumerator HandleDamageCoroutine(float _timePush, float _force)
    {
        while (_timePush > 0)
        {
           var _pushVelocity = -model.forward * _force;
            characterController.Move(_pushVelocity * Time.deltaTime + new Vector3(0f, -9.81f, 0f) * Time.deltaTime);
            
            _timePush -= Time.deltaTime;
            yield return null;
        }
    }
    public void Die()
    {
        CanControl = false;
        characterController.enabled = false;
        _jumpVelocity = -9.81f;
        if(_handleDamageCoroutine != null) StopCoroutine(_handleDamageCoroutine);
        if (IsGrounded)
        {
            Invoke(nameof(DeadDissolve), 2f);
            animator.SetBool(IDDead, true);
            return;
        }
        DeadDissolve();
    }
    private void DeadDissolve()
    {
        setEmission.ChangeCurrentIntensity(-3f);   
        setEmission.ChangeIntensitySet(5f);
        setEmission.ChangeDurationApply(.15f);
        setEmission.Apply();
        
        setDissolve.ChangeCurrentValue(0);
        setDissolve.ChangeValueSet(1);
        setDissolve.ChangeDurationApply(3f);
        setDissolve.Apply();
    }
    #endregion

    
    #region Event Callback
    public void OnDashEvent() => E_Dash?.Invoke();
    protected void OnSkillCooldownEvent () => E_SkillCD?.Invoke(PlayerConfig.SkillCD);
    protected void OnSpecialCooldownEvent () => E_SpecialCD?.Invoke(PlayerConfig.SpecialCD);
    #endregion
    
    


    /// <summary>
    /// Khi nhận sát thương, nếu nhân vật cần thực hiện hành vi thì Override lại
    /// </summary>
    protected virtual void HandleDamage() { }
    
    /// <summary>
    /// Giải phóng tất cả trạng thái khi nhảy, lướt,
    /// </summary>
    public abstract void ReleaseAction();    
    public abstract void CalculateDMG_NA();
    public abstract void CalculateDMG_CA();
    public abstract void CalculateDMG_EK();
    public abstract void CalculateDMG_EB();
    
}


[Serializable]
public struct EffectOffset
{
    public Vector3 position;
    public Vector3 rotation;
}