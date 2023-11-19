using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class nhận tất cả giá trị đầu vào của user
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputs : MonoBehaviour
{
    /*
        - Để nhận giá trị từ EventInput: Các function cần có từ khóa 'On' phía trước tên Actions đã setup trong InputAsset
        + Ví dụ: Action: Move -> Function: OnMove
     */
    
    private PlayerControl _playerControl;


    private void Awake()
    {
        _playerControl = new PlayerControl();
    }

    private void OnEnable()
    {
        _playerControl.PlayerControls.Enable();
        
        _playerControl.PlayerControls.SwitchMode.started  += OnChangeStatePressed;
        _playerControl.PlayerControls.SwitchMode.canceled += OnChangeStatePressed;
        
        _playerControl.PlayerControls.Jump.started  += OnJumpPressed;
        _playerControl.PlayerControls.Jump.canceled += OnJumpPressed;
        
        _playerControl.PlayerControls.Dash.started  += OnDashPressed;
        _playerControl.PlayerControls.Dash.canceled += OnDashPressed;
        
        _playerControl.PlayerControls.Attack.started += OnAttackPressed;
        _playerControl.PlayerControls.Attack.canceled += OnAttackPressed;
        
        _playerControl.PlayerControls.Skill.started += OnSkillPressed;
        _playerControl.PlayerControls.Skill.canceled += OnSkillPressed;
        
        _playerControl.PlayerControls.SkillSpecial.started += OnSkillSpecialPressed;
        _playerControl.PlayerControls.SkillSpecial.canceled += OnSkillSpecialPressed;
        
    }
    private void OnDisable()
    {
        _playerControl.PlayerControls.SwitchMode.started  -= OnChangeStatePressed;
        _playerControl.PlayerControls.SwitchMode.canceled -= OnChangeStatePressed;
        
        _playerControl.PlayerControls.Jump.started  -= OnJumpPressed;
        _playerControl.PlayerControls.Jump.canceled -= OnJumpPressed;
        
        _playerControl.PlayerControls.Dash.started  -= OnDashPressed;
        _playerControl.PlayerControls.Dash.canceled -= OnDashPressed;
        
        _playerControl.PlayerControls.Attack.started -= OnAttackPressed;
        _playerControl.PlayerControls.Attack.canceled -= OnAttackPressed;
        
        _playerControl.PlayerControls.Skill.started -= OnSkillPressed;
        _playerControl.PlayerControls.Skill.canceled -= OnSkillPressed;
        
        _playerControl.PlayerControls.SkillSpecial.started -= OnSkillSpecialPressed;
        _playerControl.PlayerControls.SkillSpecial.canceled -= OnSkillSpecialPressed;
        
        _playerControl.PlayerControls.Disable();
    }


 
    /// <summary>
    /// Nhận giá trị của 4 phím: A W S D
    /// </summary>
    public Vector2 move;
    private void OnMove(InputValue value) => move = value.Get<Vector2>();


    /// <summary>
    /// Chuyển đổi giữa Walk và Run
    /// </summary>
    public bool changeState;
    private void OnChangeStatePressed(InputAction.CallbackContext context) =>  changeState = context.ReadValueAsButton();

    
    /// <summary>
    /// Trả về TRUE nếu nhấn phím: ShiftLeft or RightMouseButton
    /// </summary>
    public bool leftShift;
    private void OnDashPressed(InputAction.CallbackContext context) => leftShift = context.ReadValueAsButton();
    
    
    /// <summary>
    /// Trả về TRUE nếu nhấn phím: Space
    /// </summary>
    public bool space;
    private void OnJumpPressed(InputAction.CallbackContext context) => space = context.ReadValueAsButton();

    
    /// <summary>
    /// Trả về TRUE nếu nhấn phím: LeftMouseButton
    /// </summary>
    public bool leftMouse;
    private void OnAttackPressed(InputAction.CallbackContext context) => leftMouse = context.ReadValueAsButton();

    
    /// <summary>
    /// Trả về TRUE nếu nhấn phím: E
    /// </summary>
    public bool e;
    private void OnSkillPressed(InputAction.CallbackContext context) => e = context.ReadValueAsButton();

    
    /// <summary>
    /// Trả về TRUE nếu nhấn phím: Q
    /// </summary>
    public bool q;
    private void OnSkillSpecialPressed(InputAction.CallbackContext context) => q = context.ReadValueAsButton();


}
