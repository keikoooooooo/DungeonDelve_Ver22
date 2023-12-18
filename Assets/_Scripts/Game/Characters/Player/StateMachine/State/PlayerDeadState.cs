using System.Collections;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine _currentContext, PlayerStateFactory factory) 
        : base(_currentContext, factory) { }

    private float _delayDissolve;
    private bool _canDelay;
    private Coroutine _handleCoroutine;
    
    public override void EnterState()
    {
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
        _machine.cinemachineFreeLook.enabled = false;
        DeadDissolve();
        if (_handleCoroutine != null) 
            _machine.StopCoroutine(_handleCoroutine);
        _handleCoroutine = _machine.StartCoroutine(HandleCoroutine());
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
        _machine.setDissolve.ChangeDurationApply(2f);
        _machine.setDissolve.Apply();
    }
    private IEnumerator HandleCoroutine()
    {
        yield return new WaitForSeconds(3f);
        LoadingPanel.Instance.Active(.7f);
        _machine.ResetDeadState();
    }
    
    
}
