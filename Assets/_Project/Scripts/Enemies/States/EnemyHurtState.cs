using UnityEngine;

/// <summary>
/// 敵人受傷狀態：短暫硬直後回到待機或追擊狀態。
/// </summary>
public class EnemyHurtState : EnemyBaseState
{
    private float hurtTimer;

    public EnemyHurtState(EnemyBase enemy, StateMachine stateMachine)
        : base(enemy, stateMachine) { }

    public override void Enter()
    {
        hurtTimer = enemy.Data.hurtDuration;
        // 停止移動
        enemy.Rb.velocity = new Vector2(0f, enemy.Rb.velocity.y);
    }

    public override void LogicUpdate()
    {
        hurtTimer -= Time.deltaTime;
        if (hurtTimer <= 0f)
        {
            // 恢復後：若玩家在偵測範圍內則追擊，否則待機
            if (enemy.IsPlayerInDetectionRange())
                stateMachine.ChangeState(new EnemyChaseState(enemy, stateMachine));
            else
                stateMachine.ChangeState(new EnemyIdleState(enemy, stateMachine));
        }
    }
}
