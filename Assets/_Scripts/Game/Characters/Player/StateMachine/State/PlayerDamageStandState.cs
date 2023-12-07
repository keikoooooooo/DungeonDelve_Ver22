using UnityEngine;

public class PlayerDamageStandState : PlayerBaseState
{
    public PlayerDamageStandState(PlayerStateMachine _currentContext, PlayerStateFactory factory) 
        : base(_currentContext, factory) { }

    
    private readonly float _force = 1.5f;
    private float _timePush;
    private Vector3 _pushVelocity;
    private readonly Vector3 _gravity = new(0f, -9.81f, 0f);
    
    public override void EnterState()
    {
        _timePush = .125f;
        _machine.animator.SetTrigger(_machine.IDDamageStand);   
    }
    public override void UpdateState()
    {
        CheckSwitchState();
        _machine.InputMovement = Vector3.zero;
        
        if(_timePush <= 0) 
            return;
        
        _pushVelocity = -_machine.model.forward * _force;
        _machine.characterController.Move(_pushVelocity * Time.deltaTime + _gravity * Time.deltaTime);
        _timePush -= Time.deltaTime;
    }
    protected override void ExitState()
    {
        _machine.animator.ResetTrigger(_machine.IDDamageStand);
    }
    public override void CheckSwitchState()
    {
        if (_machine.IsDash)
        {
            SwitchState(_factory.Dash());
        }
    }
}
