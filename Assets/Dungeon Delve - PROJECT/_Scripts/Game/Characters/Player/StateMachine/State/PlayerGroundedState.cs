using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine _currentContext, PlayerStateFactory factory)
        : base(_currentContext, factory)
    {
        _isRoolState = true;
        SetChildState(_factory.Idle());
    }

    public override void EnterState()
    {
        _machine.JumpVelocity = -9.81f;
    }
    protected override void UpdateState()
    {
        HandleRotation();
        
        CheckSwitchState();
    }
    public override void CheckSwitchState()
    {
        if (_machine.IsJump)
        {
            SwitchState(_factory.Jump());
        }
    }
    private void HandleRotation()
    {
        if (_machine.InputMovement == Vector3.zero || !_machine.CanRotation)
            return;

        var rotation = Quaternion.LookRotation(_machine.InputMovement, Vector3.up);
        _machine.model.rotation = Quaternion.RotateTowards(_machine.model.rotation, rotation, 1000f * Time.deltaTime);
    }
    
}
