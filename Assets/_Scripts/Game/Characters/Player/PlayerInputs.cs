using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    /*
        - Để nhận giá trị từ EventInput: Các function cần có từ khóa 'On' phía trước tên Actions đã setup trong InputAsset
        + Ví dụ: Action: Move -> Function: OnMove
     */
    
    private Inputs _playerInput;

    private void Awake()
    {
        _playerInput = new Inputs();
    }

    private void OnEnable()
    {
        _playerInput.Player.Enable();
        
        _playerInput.Player.Move.performed  += OnMovePressed;
        _playerInput.Player.Move.canceled += OnMovePressed;
        
        _playerInput.Player.SwitchMode.started  += OnChangeStatePressed;
        _playerInput.Player.SwitchMode.canceled += OnChangeStatePressed;
        
        _playerInput.Player.Jump.started  += OnJumpPressed;
        _playerInput.Player.Jump.canceled += OnJumpPressed;
        
        _playerInput.Player.Dash.started  += OnDashPressed;
        _playerInput.Player.Dash.canceled += OnDashPressed;
        
        _playerInput.Player.Attack.started += OnAttackPressed;
        _playerInput.Player.Attack.canceled += OnAttackPressed;
        
        _playerInput.Player.Skill.started += OnSkillPressed;
        _playerInput.Player.Skill.canceled += OnSkillPressed;
        
        _playerInput.Player.SkillSpecial.started += OnSkillSpecialPressed;
        _playerInput.Player.SkillSpecial.canceled += OnSkillSpecialPressed;
        
    }
    private void OnDisable()
    {
        _playerInput.Player.Move.performed  -= OnMovePressed;
        _playerInput.Player.Move.canceled -= OnMovePressed;
        
        _playerInput.Player.SwitchMode.started  -= OnChangeStatePressed;
        _playerInput.Player.SwitchMode.canceled -= OnChangeStatePressed;
        
        _playerInput.Player.Jump.started  -= OnJumpPressed;
        _playerInput.Player.Jump.canceled -= OnJumpPressed;
        
        _playerInput.Player.Dash.started  -= OnDashPressed;
        _playerInput.Player.Dash.canceled -= OnDashPressed;
        
        _playerInput.Player.Attack.started -= OnAttackPressed;
        _playerInput.Player.Attack.canceled -= OnAttackPressed;
        
        _playerInput.Player.Skill.started -= OnSkillPressed;
        _playerInput.Player.Skill.canceled -= OnSkillPressed;
        
        _playerInput.Player.SkillSpecial.started -= OnSkillSpecialPressed;
        _playerInput.Player.SkillSpecial.canceled -= OnSkillSpecialPressed;
        
        _playerInput.Player.Disable();
    }


 
    /// <summary>
    /// Nhận giá trị của 4 phím: A W S D
    /// </summary>
    public Vector2 Move;
    private void OnMovePressed(InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();


    /// <summary>
    /// Chuyển đổi giữa Walk và Run
    /// </summary>
    public bool ChangeState;
    private void OnChangeStatePressed(InputAction.CallbackContext context) =>  ChangeState = context.ReadValueAsButton();

    
    /// <summary>
    /// Trả về TRUE nếu nhấn phím: ShiftLeft or RightMouseButton
    /// </summary>
    public bool LeftShift;
    private void OnDashPressed(InputAction.CallbackContext context) => LeftShift = context.ReadValueAsButton();
    
    
    /// <summary>
    /// Trả về TRUE nếu nhấn phím: Space
    /// </summary>
    public bool Space;
    private void OnJumpPressed(InputAction.CallbackContext context) => Space = context.ReadValueAsButton();

    
    /// <summary>
    /// Trả về TRUE nếu nhấn phím: LeftMouseButton
    /// </summary>
    public bool LeftMouse;
    private void OnAttackPressed(InputAction.CallbackContext context) => LeftMouse = context.ReadValueAsButton();

    
    /// <summary>
    /// Trả về TRUE nếu nhấn phím: E
    /// </summary>
    public bool E;
    private void OnSkillPressed(InputAction.CallbackContext context) => E = context.ReadValueAsButton();

    
    /// <summary>
    /// Trả về TRUE nếu nhấn phím: Q
    /// </summary>
    public bool Q;
    private void OnSkillSpecialPressed(InputAction.CallbackContext context) => Q = context.ReadValueAsButton();


}
