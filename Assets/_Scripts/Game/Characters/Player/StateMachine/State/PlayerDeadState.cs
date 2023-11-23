using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine _currentContext, PlayerStateFactory factory) 
        : base(_currentContext, factory) { }

    private float _delayDissolve;
    private bool _canDelay;
    
    public override void EnterState()
    {
        _machine.CanControl = false;
        _machine.characterController.enabled = false;

        if (!_machine.IsGrounded)
        {
            _canDelay = false;
            DeadDissolve(); 
            return;
        }

        _delayDissolve = 2f;
        _canDelay = true;
        _machine.animator.SetBool(_machine.IDDead, true);
    }

    public override void UpdateState()
    {
        if(!_canDelay) return;
        
        _delayDissolve = _delayDissolve > 0 ? _delayDissolve - Time.deltaTime : 0;
        if (_delayDissolve > 0) 
            return;
        
        _canDelay = false;
        DeadDissolve();
    }
    protected override void ExitState() { }
    public override void CheckSwitchState() { }
    
    private void DeadDissolve()
    {
        _machine.setEmission.ChangeCurrentIntensity(-3f);   
        _machine.setEmission.ChangeIntensitySet(5f);
        _machine.setEmission.ChangeDurationApply(.15f);
        _machine.setEmission.Apply();
        
        _machine.setDissolve.ChangeCurrentValue(0);
        _machine.setDissolve.ChangeValueSet(1);
        _machine.setDissolve.ChangeDurationApply(3f);
        _machine.setDissolve.Apply();
    }
    
}
