/// <summary>
/// 子狀態機：實作 IState，內部包含另一個 StateMachine。
/// 用於實現階層式狀態（例如 GroundedSuperState 內含 Idle/Run 子狀態）。
/// </summary>
public class SubStateMachine : IState
{
    #region 欄位

    /// <summary>內部子狀態機。</summary>
    protected StateMachine subStateMachine;

    #endregion

    #region 建構子

    /// <summary>
    /// 建立子狀態機，並提供初始子狀態。
    /// </summary>
    /// <param name="initialSubState">子狀態機的初始狀態</param>
    public SubStateMachine(IState initialSubState)
    {
        subStateMachine = new StateMachine();
        subStateMachine.Initialize(initialSubState);
    }

    #endregion

    #region IState 實作

    /// <summary>進入此超狀態時，同時進入子狀態機的當前狀態。</summary>
    public virtual void Enter()
    {
        subStateMachine.CurrentState?.Enter();
    }

    /// <summary>每幀邏輯更新，委派給子狀態機。</summary>
    public virtual void LogicUpdate()
    {
        subStateMachine.Update();
    }

    /// <summary>每固定幀物理更新，委派給子狀態機。</summary>
    public virtual void PhysicsUpdate()
    {
        subStateMachine.FixedUpdate();
    }

    /// <summary>離開此超狀態時，同時離開子狀態機的當前狀態。</summary>
    public virtual void Exit()
    {
        subStateMachine.CurrentState?.Exit();
    }

    #endregion

    #region 公開方法

    /// <summary>
    /// 切換子狀態機內的子狀態。
    /// </summary>
    /// <param name="newSubState">新的子狀態</param>
    public void ChangeSubState(IState newSubState)
    {
        subStateMachine.ChangeState(newSubState);
    }

    #endregion
}
