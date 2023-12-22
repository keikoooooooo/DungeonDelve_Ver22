using System.Collections;
using FMOD.Studio;
using UnityEngine;

public class PlayerRunFastState : PlayerBaseState
{
    public PlayerRunFastState(PlayerStateMachine _currentContext, PlayerStateFactory factory) 
        : base(_currentContext, factory) {}


    private float currentBlend;
    private Coroutine _subtractSTCoroutine;
    //
    private PLAYBACK_STATE _playbackState;
   
    public override void EnterState()
    {
        _machine._footstepsInstance = _machine.runfastFootsteps;
        currentBlend = _machine.animator.GetFloat(_machine.IDSpeed);
        _machine.animator.speed = 1.4f;
        _machine.CanIncreaseST = false;
        
        if (_subtractSTCoroutine != null)
            _machine.StopCoroutine(_subtractSTCoroutine);
        _subtractSTCoroutine = _machine.StartCoroutine(SubtractSTCoroutine());
    }
    public override void UpdateState()
    {
        _machine.AppliedMovement = _machine.InputMovement.normalized * _machine.PlayerConfig.GetRunFastSpeed();

        currentBlend = Mathf.MoveTowards(currentBlend, 1, 5f * Time.deltaTime);
        _machine.animator.SetFloat(_machine.IDSpeed, currentBlend);

        CheckSwitchState();
    }
    protected override void ExitState()
    {
        _machine._footstepsInstance.stop(STOP_MODE.ALLOWFADEOUT);
        _machine.input.LeftShift = false;
        _machine.animator.speed = 1f;
        _machine.CanIncreaseST = true;
        
        if (_subtractSTCoroutine != null)
            _machine.StopCoroutine(_subtractSTCoroutine);
    }
    public override void CheckSwitchState()
    {
        // // Kiểm tra các trạng thái khi nhân vật đang đứng dưới đất
        if (_machine.IsIdle)
        {  
            SwitchState(_factory.Idle());
        }
        else if (_machine.IsRun || _machine.Stamina.CurrentValue <= 0)
        {
            SwitchState(_factory.Run());
        }
    }
    
    private IEnumerator SubtractSTCoroutine() // giảm ST
    {
        while (_machine.Stamina.CurrentValue > 0)
        {
            _machine.Stamina.Decreases(1);
            yield return new WaitForSeconds(.07f);
        }
    }
    
}
