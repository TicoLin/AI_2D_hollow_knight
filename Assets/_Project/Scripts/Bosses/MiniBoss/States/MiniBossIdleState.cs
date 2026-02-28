using UnityEngine;

/// <summary>
/// 迷你 Boss 狀態基底類別。
/// </summary>
public class MiniBossBaseState : IState
{
    protected MiniBossController boss;
    protected StateMachine stateMachine;

    public MiniBossBaseState(MiniBossController boss, StateMachine stateMachine)
    {
        this.boss = boss;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void Exit() { }
}

/// <summary>
/// 迷你 Boss 待機狀態：短暫等待後隨機（或按順序）選擇攻擊招式。
/// </summary>
public class MiniBossIdleState : MiniBossBaseState
{
    private float idleTimer;
    private const float IdleDuration = 1.5f;

    public MiniBossIdleState(MiniBossController boss, StateMachine stateMachine)
        : base(boss, stateMachine) { }

    public override void Enter()
    {
        idleTimer = IdleDuration;
        boss.Rb.velocity = Vector2.zero;
    }

    public override void LogicUpdate()
    {
        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0f)
        {
            ChooseNextAttack();
        }
    }

    /// <summary>隨機選擇下一個攻擊招式。</summary>
    private void ChooseNextAttack()
    {
        // Phase 2 有更多選項
        int maxChoice = boss.CurrentPhase >= 2 ? 4 : 3;
        int choice = Random.Range(0, maxChoice);

        switch (choice)
        {
            case 0: stateMachine.ChangeState(boss.DashAttackState); break;
            case 1: stateMachine.ChangeState(boss.SlamState); break;
            case 2: stateMachine.ChangeState(boss.ComboState); break;
            case 3: stateMachine.ChangeState(boss.ProjectileState); break; // Phase 2 only
        }
    }
}
