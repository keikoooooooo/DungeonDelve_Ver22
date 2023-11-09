using System.Collections;
using UnityEngine;

public class PlayerRunFastState : PlayerBaseState
{
    public PlayerRunFastState(PlayerStateMachine _currentContext, PlayerStateFactory factory) 
        : base(_currentContext, factory) {}


    private float currentBlend;
    private Coroutine _subtractSTCoroutine;
    
   
    public override void EnterState()
    {
        currentBlend = _machine.animator.GetFloat(_machine.IDSpeed);
        _machine.animator.speed = 1.4f;
        _machine.CanIncreaseST = false;
        
        if (_subtractSTCoroutine != null)
            _machine.StopCoroutine(_subtractSTCoroutine);
        _subtractSTCoroutine = _machine.StartCoroutine(SubtractSTCoroutine());
    }
    public override void UpdateState()
    {
        _machine.AppliedMovement = _machine.InputMovement.normalized * _machine.PlayerConfig.RunFastSpeed;

        currentBlend = Mathf.MoveTowards(currentBlend, 1, 5f * Time.deltaTime);
        _machine.animator.SetFloat(_machine.IDSpeed, currentBlend);
        
        CheckSwitchState();
    }
    protected override void ExitState()
    {
        _machine.inputs.leftShift = false;
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
        else if (_machine.IsRun || _machine.StatusHandle.CurrentStamina <= 0)
        {
            SwitchState(_factory.Run());
        }
    }
    
    private IEnumerator SubtractSTCoroutine() // giảm ST
    {
        while (_machine.StatusHandle.CurrentStamina > 0)
        {
            _machine.StatusHandle.Subtract(1, StatusHandle.StatusType.Stamina);
            yield return new WaitForSeconds(.07f);
        }
    }
    
}
