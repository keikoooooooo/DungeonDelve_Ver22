using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Quản lí tất cả state của player
/// </summary>
public abstract class PlayerStateMachine : MonoBehaviour, IDamageable
{
    #region Variables
    [Header("BaseClass -------")]
    [Tooltip("Nhận các giá trị đầu vào của player")]
    public PlayerInputs inputs;
    
    [Tooltip("Mô hình 3D")]
    public Transform model;

    [Tooltip("Điều khiển animation"), SerializeField]
    public Animator animator;
    
    
    #region Get & Set Property
    [field: SerializeField] public PlayerConfiguration PlayerConfig { get; private set; }
    public StatusHandle StatusHandle { get; private set; }
    public CharacterController CharacterController { get; set; }
    public PlayerBaseState CurrentState { get; set; }
    public Vector3 AppliedMovement { get; set; }
    public Vector3 InputMovement { get; set; }
    
    protected bool IsGrounded => CharacterController.isGrounded;
    public bool CanMove { get; set; }
    public bool CanRotation { get; set; }
    public bool IsIdle => inputs.move.magnitude == 0;
    public bool IsJump => inputs.space && !inputs.leftShift;
    public bool IsWalk => !IsIdle && _movementState == MovementState.StateWalk;
    public bool IsRun => !IsIdle && IsGrounded && !inputs.leftShift && _movementState == MovementState.StateRun;
    public bool IsDash => inputs.leftShift && IsGrounded && StatusHandle.CurrentStamina >= PlayerConfig.DashEnergy;
    public bool CanIncreaseST { get; set; } // có thể tăng ST không ?
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
    
    // player
    protected enum MovementState
    {
        StateWalk,
        StateRun
    }
    private PlayerStateFactory _state;
    [HideInInspector] protected MovementState _movementState;
    [HideInInspector] protected Camera _mainCamera;
    [HideInInspector] private float _gravity;
    [HideInInspector] private bool _pressedJump;
    [HideInInspector] private float _jumpVelocity;
    [HideInInspector] private float _jumpCD_Temp;
    [HideInInspector] protected float _skillCD_Temp;
    [HideInInspector] protected float _specialCD_Temp;
    protected int _calculatedDamage;
    
    // Coroutine
    private Coroutine _handleSTCoroutine;
    #endregion


    private void OnEnable()
    {
        SetVariables();
    }
    private void Start()
    {
        SetReference();
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
        ResetVariables();
    }


    /// <summary>
    /// Khởi tạo giá trị biến ban đầu, và sẽ gọi hàm mỗi lần object này được OnEnable
    /// </summary>
    protected virtual void SetVariables()
    {
        _gravity = -30f;
        CanMove = true;
        CanRotation = true;
        CanIncreaseST = true;
        _movementState = MovementState.StateRun;

        StatusHandle = new StatusHandle(PlayerConfig.MaxHealth, PlayerConfig.MaxStamina);
        StatusHandle.E_Die += Die;
        
        DamageableData.Add(gameObject, this);
    }
    private void ResetVariables()
    {
        DamageableData.Remove(gameObject);
        StatusHandle.E_Die -= Die;
    }

    
    /// <summary>
    /// Khởi tạo các tham chiếu, và sẽ gọi hàm này 1 lần bằng hàm Start
    /// </summary>
    protected virtual void SetReference()
    {
        _mainCamera = Camera.main;
        CharacterController = GetComponent<CharacterController>();

        // Setup State
        _state = new PlayerStateFactory(this);
        CurrentState = _state.Idle();
        CurrentState.EnterState();

        if (_handleSTCoroutine != null)
            StopCoroutine(_handleSTCoroutine);
        _handleSTCoroutine = StartCoroutine(IncreaseSTCoroutine());
    }
    

    

    public void HandleInput()
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
        if (!CanMove || !CharacterController.enabled) 
            return;
        
        CharacterController.Move(AppliedMovement * Time.deltaTime 
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
            var totalDMG = Mathf.CeilToInt(_calculatedDamage * critDMG);
            
            _calculatedDamage = totalDMG;
            _isCrit = true;
        } 
        
        iDamageable.TakeDMG(_calculatedDamage, _isCrit);
    }
    public void TakeDMG(int _damage, bool _isCRIT)
    {   
        
        DMGPopUpGenerator.Instance.Create(transform.position, _damage, _isCRIT, false);
    }
    public void Die()
    {
        
    }
    #endregion
    
    
    public IEnumerator IncreaseSTCoroutine()
    {
        while (true)
        {
            if (StatusHandle.CurrentStamina <= 100 && CanIncreaseST)
            {
                StatusHandle.Increase(2, StatusHandle.StatusType.Stamina);
            }
            yield return new WaitForSeconds(.15f);
        }
    } 
    
    
    #region Event Callback
    public void OnDashEvent() => E_Dash?.Invoke();
    protected void OnSkillCooldownEvent () => E_SkillCD?.Invoke(PlayerConfig.SkillCD);
    protected void OnSpecialCooldownEvent () => E_SpecialCD?.Invoke(PlayerConfig.SpecialCD);
    #endregion

    
    
    /// <summary>
    /// Giải phóng tất cả trạng thái khi nhảy hoặc lướt.
    /// </summary>
    public abstract void ReleaseAction();
    
    /// <summary>
    /// Tính sát thương của Normal Attack
    /// </summary>  
    protected abstract void CalculateDMG_NA();
    /// <summary>
    /// Tính sát thương của Charged Attack
    /// </summary>
    protected abstract void CalculateDMG_CA();
    /// <summary>
    /// Tính sát thương của Elemental Skill
    /// </summary>
    protected abstract void CalculateDMG_EK();
    /// <summary>
    /// Tính sát thương của Elemental Burst
    /// </summary>
    protected abstract void CalculateDMG_EB();
    
}


[Serializable]
public struct EffectOffset
{
    public Vector3 position;
    public Vector3 rotation;
}