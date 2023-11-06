using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory) {}
    
    
    private float currentBlend;
    private float lastInputLeftShift; // thời gian nhấn phím shift 
    private bool isLeftShiftPressed; // có nhấn phím shift ?
    
    public override void EnterState()
    {
        currentBlend = _machine.animator.GetFloat(_machine.IDSpeed);
    }
    public override void UpdateState()
    {
        _machine.AppliedMovement = _machine.InputMovement.normalized * _machine.PlayerConfig.runSpeed;
        
        currentBlend = Mathf.MoveTowards(currentBlend, 1, 5f * Time.deltaTime);
        _machine.animator.SetFloat(_machine.IDSpeed, currentBlend);
        
        
        CheckSwitchState();
    }
    protected override void ExitState()
    { }
    
    public override void CheckSwitchState()
    {
        // // Kiểm tra các trạng thái khi nhân vật đang đứng dưới đất
        if (_machine.IsIdle)
        {  
            SwitchState(_factory.Idle());
        }
        else if (_machine.IsWalk)
        {
            SwitchState(_factory.Walk());
        }

        if (_machine.inputs.leftShift)
        {
            isLeftShiftPressed = true;
            lastInputLeftShift += Time.deltaTime;
        }
        switch (isLeftShiftPressed)
        {
            case true when !_machine.inputs.leftShift && lastInputLeftShift <= .2f:
                if(_machine.CurrentST >= _machine.PlayerConfig.dashEnergy) 
                    SwitchState(_factory.Dash());
                
                lastInputLeftShift = 0;
                isLeftShiftPressed = false;
                _machine.inputs.leftShift = false;
                break;
            
            case true when _machine.inputs.leftShift && lastInputLeftShift >= .4f:
                SwitchState(_factory.RunFast());
                
                lastInputLeftShift = 0;
                isLeftShiftPressed = false;
                break;
        }  
        
    }
}