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

    private PlayerState _playerState;
    private readonly Dictionary<PlayerState, PlayerBaseState> _states = new Dictionary<PlayerState, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        var context = currentContext;
        
        _states.Add(PlayerState.Idle, new PlayerIdleState(context, this));
        _states.Add(PlayerState.Walk, new PlayerWalkState(context, this));
        _states.Add(PlayerState.Run, new PlayerRunState(context, this));
        _states.Add(PlayerState.RunFast, new PlayerRunFastState(context, this));
        _states.Add(PlayerState.Dash, new PlayerDashState(context, this));
    }

    public PlayerBaseState Idle()    => _states[PlayerState.Idle];
    public PlayerBaseState Walk()    => _states[PlayerState.Walk];
    public PlayerBaseState Run()     => _states[PlayerState.Run];
    public PlayerBaseState RunFast() => _states[PlayerState.RunFast];
    public PlayerBaseState Dash()    => _states[PlayerState.Dash];

    
}
