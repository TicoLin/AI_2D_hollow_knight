using UnityEngine;

/// <summary>
/// 敵人攻擊狀態：對玩家造成傷害，結束後回到追擊或待機狀態。
/// </summary>
public class EnemyAttackState : EnemyBaseState
{
    private float attackCooldownTimer;

    public EnemyAttackState(EnemyBase enemy, StateMachine stateMachine)
        : base(enemy, stateMachine) { }

    public override void Enter()
    {
        attackCooldownTimer = enemy.Data.attackCooldown;

        // 執行攻擊：對範圍內的玩家造成傷害
        PerformAttack();
    }

    public override void LogicUpdate()
    {
        attackCooldownTimer -= Time.deltaTime;
        if (attackCooldownTimer <= 0f)
        {
            // 攻擊冷卻結束：若玩家仍在追擊範圍繼續追，否則回待機
            if (enemy.IsPlayerInDetectionRange())
                stateMachine.ChangeState(new EnemyChaseState(enemy, stateMachine));
            else
                stateMachine.ChangeState(new EnemyIdleState(enemy, stateMachine));
        }
    }

    /// <summary>執行攻擊判定。</summary>
    private void PerformAttack()
    {
        if (enemy.PlayerTransform == null) return;

        float dist = Vector2.Distance(enemy.transform.position, enemy.PlayerTransform.position);
        if (dist <= enemy.Data.attackRange)
        {
            IDamageable damageable = enemy.PlayerTransform.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Vector2 dir = (enemy.PlayerTransform.position - enemy.transform.position).normalized;
                damageable.TakeDamage(enemy.Data.attackDamage, dir);
            }
        }
    }
}
