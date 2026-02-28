/// <summary>
/// 狀態機介面，所有狀態皆需實作此介面。
/// </summary>
public interface IState
{
    /// <summary>進入此狀態時呼叫（初始化）。</summary>
    void Enter();

    /// <summary>每幀呼叫（在 Update 中）。</summary>
    void LogicUpdate();

    /// <summary>每固定幀呼叫（在 FixedUpdate 中）。</summary>
    void PhysicsUpdate();

    /// <summary>離開此狀態時呼叫（清理）。</summary>
    void Exit();
}
