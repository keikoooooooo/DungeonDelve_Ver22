using UnityEngine;

public class PlayerDamageFallState : PlayerBaseState
{
    public PlayerDamageFallState(PlayerStateMachine _currentContext, PlayerStateFactory factory)
        : base(_currentContext, factory) { }


    private readonly float _force = 6f;
    private float _timePush;
    private Vector3 _pushVelocity;
    private readonly Vector3 _gravity = new(0f, -9.81f, 0f);
    private bool _canMoveBehind;
    
    public override void EnterState()
    {
        // _canMoveBehind = !_machine.animator.IsTag("Damage", 1);
        // if (!_canMoveBehind) return;
        _timePush = .35f;
        _machine.animator.SetTrigger(_machine.IDDamageFall);
        _machine.voice.PlayHeavyHit();
    }
    public override void UpdateState()
    {
        CheckSwitchState();
        if(_timePush <= 0 || !_canMoveBehind)
            return;
        
        _pushVelocity = -_machine.model.forward * _force;
        _machine.characterController.Move(_pushVelocity * Time.deltaTime + _gravity * Time.deltaTime);
        _timePush -= Time.deltaTime;
    }
    protected override void ExitState()
    {
        _machine.SetPlayerInputState(true);
        _machine.ResetDamageTrigger();
    }
    public override void CheckSwitchState()
    {
        if (!_machine.IsDash) return;
        SwitchState(_factory.Dash());
    }

}
