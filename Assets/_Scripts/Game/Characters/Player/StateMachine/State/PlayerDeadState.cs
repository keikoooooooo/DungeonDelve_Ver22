using System.Collections;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine _currentContext, PlayerStateFactory factory) 
        : base(_currentContext, factory) { }
    
    private Coroutine _revivalCorouine;
    private readonly float _revivalTime = 5f;
    
    public override void EnterState()
    {
        if (_revivalCorouine != null)
            _machine.StopCoroutine(_revivalCorouine);
        _revivalCorouine = _machine.StartCoroutine(RevivalCoroutine());
    }
    public override void UpdateState() { }
    protected override void ExitState()
    {
        _machine.animator.SetFloat(_machine.IDSpeed, 0);
    }
    public override void CheckSwitchState() { }
    

    private IEnumerator RevivalCoroutine()
    {
        EnemyTracker.Clear();
        SetInputState(false);
        
        _machine.voice.PlayDie();
        _machine.CallbackDieEvent();
        _machine.animator.SetBool(_machine.IDDead, true);
        _machine.animator.SetFloat(_machine.IDSpeed, 0);
        _machine.cinemachineFreeLook.enabled = false;
        
        yield return new WaitForSeconds(2f);
        DeadDissolve();
        _machine.CallbackRevivalTimeEvent(_revivalTime);
        
        yield return new WaitForSeconds(_revivalTime);
        LoadingPanel.Instance.Active(.7f);
        _machine.CallbackRevivalTimeEvent(0);
        _machine.animator.SetBool(_machine.IDDead,false);
        RevialDissolve();
        SetTransform();
        InitStatus();
        
        yield return new WaitForSeconds(1f);
        _machine.ReleaseDamageState();
        SetFreeLookCamera();
        SetInputState(true);
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
    }
    private void RevialDissolve()
    {
        _machine.setDissolve.ChangeDurationApply(1f);
        _machine.setDissolve.ChangeCurrentValue(1f);
        _machine.setDissolve.ChangeValueSet(0f);
        _machine.setDissolve.Apply();
        
        _machine.setEmission.ChangeIntensitySet(0);
        _machine.setEmission.ChangeDurationApply(0);
        _machine.setEmission.Apply();
    }
    private void SetInputState(bool _state)
    {
        if (_state)
        {
            _machine.input.PlayerInput.Enable();
            GUI_Inputs.InputAction.Enable();
            return;
        }
        _machine.input.PlayerInput.Disable();
        GUI_Inputs.InputAction.Disable();
    }
    private void SetTransform()
    {
        _machine.characterController.enabled = false;
        _machine.transform.position = Vector3.zero;
        _machine.characterController.enabled = true;
        _machine.model.rotation = Quaternion.Euler(Vector3.zero);
    }
    private void SetFreeLookCamera()
    {
        _machine.cinemachineFreeLook.enabled = true;
        _machine.cinemachineFreeLook.m_XAxis.Value = 0;
        _machine.cinemachineFreeLook.m_YAxis.Value = .5f;
    }
    private void InitStatus()
    {
        var _hp = _machine.PlayerConfig.GetHP();
        var _st = _machine.PlayerConfig.GetST();
        _machine.Health.InitValue(_hp, _hp);
        _machine.Stamina.InitValue(_st, _st);
    }
    
}
