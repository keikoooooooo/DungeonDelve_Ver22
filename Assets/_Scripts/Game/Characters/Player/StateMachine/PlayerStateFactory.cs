using System.Collections.Generic;

public class PlayerStateFactory
{
    public enum PlayerState
    {
        Idle,
        Walk,
        Run,
        RunFast,
        Dash
    }
    
    private readonly Dictionary<PlayerState, PlayerBaseState> _states = new ();

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _states.Add(PlayerState.Idle, new PlayerIdleState(currentContext, this));
        _states.Add(PlayerState.Walk, new PlayerWalkState(currentContext, this));
        _states.Add(PlayerState.Run, new PlayerRunState(currentContext, this));
        _states.Add(PlayerState.RunFast, new PlayerRunFastState(currentContext, this));
        _states.Add(PlayerState.Dash, new PlayerDashState(currentContext, this));
    }

    public PlayerBaseState Idle()    => _states[PlayerState.Idle];
    public PlayerBaseState Walk()    => _states[PlayerState.Walk];
    public PlayerBaseState Run()     => _states[PlayerState.Run];
    public PlayerBaseState RunFast() => _states[PlayerState.RunFast];
    public PlayerBaseState Dash()    => _states[PlayerState.Dash];

    
}
