/// <summary>
/// Định nghĩa các hành vi mà tất cả các State khác đều có.
/// </summary>
public abstract class PlayerBaseState
{
    
    protected PlayerBaseState(PlayerStateMachine _currentContext, PlayerStateFactory factory)
    {
        _machine = _currentContext;
        _factory = factory;
    }
    
    protected readonly PlayerStateMachine _machine; 
    protected readonly PlayerStateFactory _factory;
    
    
    #region Abstract Methods
    /// <summary>
    /// Khi đổi một trạng thái mới, hàm này sẽ được gọi để bắt đầu trạng thái hiện tại
    /// </summary>
    public abstract void EnterState();

    
    /// <summary>
    /// Hàm này sẽ được gọi mỗi frame của trạng thái hiện tại
    /// </summary>
    public abstract void UpdateState();
    
    
    /// <summary>
    /// Khi đổi một trạng thái mới, hàm này sẽ được gọi để thoát trạng thái hiện tại
    /// </summary>
    protected abstract void ExitState();
    
    
    /// <summary>
    /// Kiểm tra các điều kiện nếu thỏa mãn -> đổi trạng thái, hàm này cần được gọi trong UpdateState();
    /// </summary>
    public abstract void CheckSwitchState();
    #endregion
    
    
    /// <summary>
    /// Đổi và cập nhật trạng thái mới vào StateMachine của player
    /// </summary>
    /// <param name="newState"> Trạng thái cần đổi </param>
    protected void SwitchState(PlayerBaseState newState)
    {
        // Thoát State, bất kể trạng thái nào.
        ExitState();
        
        // Khởi chạy state mới
        newState.EnterState();

        // Đổi state hiện tại thành state mới trên script quản lí state
        _machine.CurrentState = newState;
    }
    

}

