/// <summary>
/// 通用狀態機，管理狀態的切換與更新。
/// </summary>
public class StateMachine
{
    #region 屬性

    /// <summary>目前正在執行的狀態。</summary>
    public IState CurrentState { get; private set; }

    #endregion

    #region 公開方法

    /// <summary>
    /// 初始化狀態機，設定起始狀態。
    /// </summary>
    /// <param name="startingState">起始狀態</param>
    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    /// <summary>
    /// 切換至新的狀態（先離開舊狀態，再進入新狀態）。
    /// </summary>
    /// <param name="newState">目標新狀態</param>
    public void ChangeState(IState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    /// <summary>
    /// 邏輯更新，應在 MonoBehaviour.Update() 中呼叫。
    /// </summary>
    public void Update()
    {
        CurrentState?.LogicUpdate();
    }

    /// <summary>
    /// 物理更新，應在 MonoBehaviour.FixedUpdate() 中呼叫。
    /// </summary>
    public void FixedUpdate()
    {
        CurrentState?.PhysicsUpdate();
    }

    #endregion
}
