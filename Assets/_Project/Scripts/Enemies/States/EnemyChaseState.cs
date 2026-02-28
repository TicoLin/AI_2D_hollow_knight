using UnityEngine;

/// <summary>
/// 敵人追擊狀態：朝玩家移動，進入攻擊範圍後切換至攻擊狀態。
/// </summary>
public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyBase enemy, StateMachine stateMachine)
        : base(enemy, stateMachine) { }

    public override void LogicUpdate()
    {
        // 玩家太遠 → 回到待機
        if (enemy.PlayerTransform == null ||
            Vector2.Distance(enemy.transform.position, enemy.PlayerTransform.position) > enemy.Data.loseTargetRange)
        {
            stateMachine.ChangeState(new EnemyIdleState(enemy, stateMachine));
            return;
        }

        // 進入攻擊範圍 → 攻擊
        if (enemy.IsPlayerInAttackRange())
        {
            stateMachine.ChangeState(new EnemyAttackState(enemy, stateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        if (enemy.PlayerTransform == null) return;

        float direction = enemy.PlayerTransform.position.x > enemy.transform.position.x ? 1f : -1f;
        enemy.Rb.velocity = new Vector2(direction * enemy.Data.chaseSpeed, enemy.Rb.velocity.y);

        // 翻轉面向玩家
        if ((direction > 0 && enemy.transform.localScale.x < 0) ||
            (direction < 0 && enemy.transform.localScale.x > 0))
        {
            enemy.Flip();
        }
    }
}
