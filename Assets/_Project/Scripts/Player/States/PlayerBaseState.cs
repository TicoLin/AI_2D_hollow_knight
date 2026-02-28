/// <summary>
/// 玩家狀態基底類別：所有玩家狀態皆繼承此類別。
/// 持有 PlayerController、StateMachine、PlayerData 引用，並提供轉移邏輯的虛方法。
/// </summary>
public class PlayerBaseState : IState
{
    #region 欄位

    /// <summary>玩家主控制器引用。</summary>
    protected PlayerController controller;

    /// <summary>狀態機引用。</summary>
    protected StateMachine stateMachine;

    /// <summary>玩家數值配置引用。</summary>
    protected PlayerData data;

    #endregion

    #region 建構子

    /// <summary>
    /// 建立玩家基底狀態。
    /// </summary>
    /// <param name="controller">玩家主控制器</param>
    /// <param name="stateMachine">狀態機</param>
    /// <param name="data">玩家數值配置</param>
    public PlayerBaseState(PlayerController controller, StateMachine stateMachine, PlayerData data)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
        this.data = data;
    }

    #endregion

    #region IState 實作

    /// <summary>進入此狀態（子類別覆寫以初始化）。</summary>
    public virtual void Enter() { }

    /// <summary>每幀邏輯更新（子類別覆寫）。</summary>
    public virtual void LogicUpdate()
    {
        CheckTransitions();
    }

    /// <summary>每固定幀物理更新（子類別覆寫）。</summary>
    public virtual void PhysicsUpdate() { }

    /// <summary>離開此狀態（子類別覆寫以清理）。</summary>
    public virtual void Exit() { }

    #endregion

    #region 虛方法

    /// <summary>
    /// 檢查狀態轉移條件（子類別覆寫以定義轉移邏輯）。
    /// </summary>
    protected virtual void CheckTransitions() { }

    #endregion

    #region 共用轉移判斷

    /// <summary>嘗試轉移至衝刺狀態（若有衝刺輸入且不在冷卻中）。</summary>
    protected bool TryDash()
    {
        if (controller.DashInputDown && controller.CanDash)
        {
            stateMachine.ChangeState(controller.DashState);
            return true;
        }
        return false;
    }

    /// <summary>嘗試轉移至攻擊狀態（若有攻擊輸入）。</summary>
    protected bool TryAttack()
    {
        if (controller.AttackInputDown)
        {
            stateMachine.ChangeState(controller.AttackState);
            return true;
        }
        return false;
    }

    #endregion
}
