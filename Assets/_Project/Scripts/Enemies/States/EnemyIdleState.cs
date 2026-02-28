using UnityEngine;

/// <summary>
/// 敵人待機/巡邏狀態：在兩個巡邏點之間來回移動，偵測玩家後切換至追擊。
/// </summary>
public class EnemyIdleState : EnemyBaseState
{
    private float waitTimer;
    private bool isWaiting;
    private bool movingRight = true;

    public EnemyIdleState(EnemyBase enemy, StateMachine stateMachine)
        : base(enemy, stateMachine) { }

    public override void Enter()
    {
        isWaiting = false;
        waitTimer = 0f;
    }

    public override void LogicUpdate()
    {
        // 偵測到玩家 → 追擊
        if (enemy.IsPlayerInDetectionRange())
        {
            stateMachine.ChangeState(new EnemyChaseState(enemy, stateMachine));
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        if (isWaiting)
        {
            waitTimer -= Time.fixedDeltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                movingRight = !movingRight;
                enemy.Flip();
            }
            return;
        }

        // 移動至目標巡邏點
        Transform target = movingRight ? enemy.PatrolRight : enemy.PatrolLeft;
        if (target == null) return;

        float direction = movingRight ? 1f : -1f;
        enemy.Rb.velocity = new Vector2(direction * enemy.Data.moveSpeed, enemy.Rb.velocity.y);

        // 到達巡邏點 → 等待後折返
        float distToTarget = Mathf.Abs(enemy.transform.position.x - target.position.x);
        if (distToTarget < 0.2f)
        {
            enemy.Rb.velocity = new Vector2(0f, enemy.Rb.velocity.y);
            isWaiting = true;
            waitTimer = enemy.Data.patrolWaitTime;
        }
    }
}
