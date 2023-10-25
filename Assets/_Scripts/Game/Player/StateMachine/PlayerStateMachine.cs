using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Quản lí tất cả state của player
/// </summary>
public abstract class PlayerStateMachine : MonoBehaviour
{
    #region Variables
    [Header("BaseClass -------")]
    
    [Tooltip("Nhận các giá trị đầu vào của player")]
    public PlayerInputs inputs;
    
    [Tooltip("Mô hình 3D")]
    public Transform model;
    
    [FormerlySerializedAs("playerChecker")] [Tooltip("Khu vực check enemy")]
    public PlayerSensor playerSensor;

    [Tooltip("Kiểm tra mặt đất"), SerializeField]
    protected GroundCheck groundCheck;

    [Tooltip("Điều khiển animation"), SerializeField]
    public Animator animator;
    
    
    #region Get & Set Property 
    [field: SerializeField] public PlayerStatsSO Stats { get; private set; }
    public CharacterController CharacterController { get; set; }

    public PlayerBaseState CurrentState { get; set; }
    
    public Vector3 Forward => model.forward;
    public Vector3 AppliedMovement { get; set; }
    public Vector3 InputMovement { get; set; }
    
    protected bool IsGrounded => groundCheck.IsGrounded;
    public bool CanMove { get; set; }
    public bool CanRotation { get; set; }
    public bool IsIdle => inputs.move.magnitude == 0;
    public bool IsJump => inputs.space;
    public bool IsWalk => !IsIdle && _movementState == MovementState.StateWalk;
    public bool IsRun => !IsIdle && IsGrounded && !inputs.leftShift && _movementState == MovementState.StateRun;
    public bool IsDash => inputs.leftShift && IsGrounded && CurrentST >= Stats.dashEnergy;
    
    public int CurrentHP { get; set; }  // máu hiện tại
    public int CurrentST { get; set; }  // năng lượng hiện tại
    public bool CanIncreaseST { get; set; }  // có thể tăng ST không ?
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
    [HideInInspector] public int IDDash = Animator.StringToHash("Dash");                    //  lướt
    [HideInInspector] public int IDAttackCombo = Animator.StringToHash("AttackCombo");      //  tấn công
    [HideInInspector] public int IDAttackHolding = Animator.StringToHash("AttackHolding");  //  tấn công
    [HideInInspector] public int IDSkill = Animator.StringToHash("Skill");                  //  kỹ năng
    [HideInInspector] public int IDSpecial = Animator.StringToHash("Special");              //  kỹ năng đặc biệt
    #endregion
    
    // Events
    public event Action E_Dash;
    public event Action<int> E_CurrentST; 
    public event Action<int> E_CurrentHP;
    public event Action<float> E_SkillCooldown; 
    public event Action<float> E_SpecialCooldown;
    
    // player
    public enum MovementState
    {
        StateWalk,
        StateRun
    }
    private PlayerStateFactory _state;
    private MovementState _movementState;
    [HideInInspector] protected Camera _mainCamera;
    
    [HideInInspector] protected float _skillCooldown;
    [HideInInspector] protected float _specialCooldown;
    [HideInInspector] private float _jumpVelocity;
    [HideInInspector] private float _jumpTimeOut;
    [HideInInspector] private float _fallTimeOut;
    [HideInInspector] private float _gravity;
    
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


    /// <summary>
    /// Đặt các giá trị biến ban đầu, và sẽ gọi hàm mỗi lần object này được OnEnable
    /// </summary>
    protected virtual void SetVariables()
    {
        _fallTimeOut = Stats.fallTimeOut;
        _gravity = -30f;
        CurrentST = 100;
        CanMove = true;
        CanRotation = true;
        CanIncreaseST = true;
        _movementState = MovementState.StateRun;
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
        
        // Thời gian để tấn công
        _skillCooldown = _skillCooldown > 0 ? _skillCooldown - Time.deltaTime : 0;
        _specialCooldown = _specialCooldown > 0 ? _specialCooldown - Time.deltaTime : 0;
        
        // Chuyển đổi mode: Walk <=> Run
        if (inputs.changeState)
        {
            inputs.changeState = false;
            _movementState = _movementState == MovementState.StateRun ? MovementState.StateWalk : MovementState.StateRun;
        }
    }
    private void HandleMovement()
    {
        if (!CanMove) 
            return;
        
        CharacterController.Move(AppliedMovement * Time.deltaTime 
                                 + new Vector3(0f, _jumpVelocity, 0f) * Time.deltaTime);
    }
    private void HandleJumping()
    {
        if (IsGrounded && _jumpVelocity < 0)
            _jumpVelocity = -9f;
        
        if (IsGrounded)
        {
            animator.SetBool(IDJump, false);
            animator.SetBool(IDFall, false);
            
            _fallTimeOut = Stats.fallTimeOut;
            _jumpTimeOut = _jumpTimeOut > 0 ? _jumpTimeOut - Time.deltaTime : 0;
            if (_jumpTimeOut <= 0 && IsJump)
            {
                animator.SetBool(IDJump, true);
                _jumpVelocity = Mathf.Sqrt(Stats.jumpHeight * -2f * _gravity);
                inputs.space = false;
                ReleaseAction();
            }
        }
        else
        {
            inputs.space = false;
            
            _jumpTimeOut = Stats.jumpTimeOut;
            _fallTimeOut = _fallTimeOut > 0 ? _fallTimeOut - Time.deltaTime : 0;
            if (_fallTimeOut <= 0)
            {
                if(IsIdle)
                    animator.SetBool(IDFall, true);
            }
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
            if (CurrentST < 100 && CanIncreaseST)
            {
                CurrentST = Mathf.Clamp(CurrentST + 2, 0, 100);
                OnCurrentSTEvent();
            }
            yield return new WaitForSeconds(.15f);
        }
    } 
    
    
    #region Event Callback
    public void OnDashEvent() => E_Dash?.Invoke();
    // public void OnCurrentHPEvent() => E_CurrentHP?.Invoke();
    public void OnCurrentSTEvent () => E_CurrentST?.Invoke(CurrentST);
    protected void OnSkillCooldownEvent () => E_SkillCooldown?.Invoke(Stats.skillCooldown);
    protected void OnSpecialCooldownEvent () => E_SpecialCooldown?.Invoke(Stats.specialCooldown);
    #endregion

    
    /// <summary>
    /// Giải phóng tất cả trạng thái khi nhảy hoặc lướt.
    /// </summary>
    public abstract void ReleaseAction();


}
