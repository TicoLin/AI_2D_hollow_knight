/// <summary>
/// 敵人狀態基底類別：所有敵人狀態繼承此類別。
/// </summary>
public class EnemyBaseState : IState
{
    #region 欄位

    protected EnemyBase enemy;
    protected StateMachine stateMachine;

    #endregion

    #region 建構子

    public EnemyBaseState(EnemyBase enemy, StateMachine stateMachine)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
    }

    #endregion

    #region IState 實作

    public virtual void Enter() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void Exit() { }

    #endregion
}
