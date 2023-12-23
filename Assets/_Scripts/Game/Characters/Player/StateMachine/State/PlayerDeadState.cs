using System.Collections;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine _currentContext, PlayerStateFactory factory) 
        : base(_currentContext, factory) { }

    private float _delayDissolve;
    private float _delayResetState;
    private bool _isDie;
    private bool _canSetDissolve;
    
    public override void EnterState()
    {
        _isDie = true;
        _canSetDissolve = true;
        _machine.voice.PlayDie();
        _machine.characterController.enabled = false;
        _delayResetState = 2.5f;

        if (!_machine.IsGrounded)
        {
            _canSetDissolve = false;
  
            _delayDissolve = 0f;
            DeadDissolve(); 
        }
        else
        {
            _delayDissolve = 2f;
            _machine.animator.SetBool(_machine.IDDead, true);
        }
    }
    public override void UpdateState()
    {
        if (_canSetDissolve)
        {
            _delayDissolve = _delayDissolve > 0 ? _delayDissolve - Time.deltaTime : 0;
            if (_delayDissolve > 0) return;
            DeadDissolve();
        }
        
        CheckSwitchState();
    }
    protected override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (!_isDie) return;
        
        _delayResetState = _delayResetState > 0 ? _delayResetState - Time.deltaTime : 0;
        if (_delayResetState > 0) 
            return;
        
        LoadingPanel.Instance.Active(.7f);
        _machine.ResetDeadState();
        _isDie = false;
    }
    private void DeadDissolve()
    {
        _machine.setEmission.ChangeCurrentIntensity(-3f);   
        _machine.setEmission.ChangeIntensitySet(5f);
        _machine.setEmission.ChangeDurationApply(.15f);
        _machine.setEmission.Apply();
        
        _machine.setDissolve.ChangeCurrentValue(0);
        _machine.setDissolve.ChangeValueSet(1);
        _machine.setDissolve.ChangeDurationApply(2f);
        _machine.setDissolve.Apply();
        
        _canSetDissolve = false;
    }
    
    
}
