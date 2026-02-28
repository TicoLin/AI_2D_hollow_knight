using UnityEngine;

/// <summary>
/// 迷你 Boss 投射物攻擊狀態（Phase 2 限定）：發射投射物朝玩家。
/// </summary>
public class MiniBossProjectileState : MiniBossBaseState
{
    private const float StateDuration = 1.0f;
    private float stateTimer;

    public MiniBossProjectileState(MiniBossController boss, StateMachine stateMachine)
        : base(boss, stateMachine) { }

    public override void Enter()
    {
        stateTimer = StateDuration;
        FireProjectile();
    }

    public override void LogicUpdate()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            stateMachine.ChangeState(boss.IdleState);
        }
    }

    /// <summary>發射投射物朝玩家位置。</summary>
    private void FireProjectile()
    {
        if (boss.ProjectilePrefab == null || boss.PlayerTransform == null) return;

        Transform spawnPoint = boss.ProjectileSpawnPoint != null
            ? boss.ProjectileSpawnPoint
            : boss.transform;

        // 計算朝玩家的方向
        Vector2 direction = (boss.PlayerTransform.position - spawnPoint.position).normalized;

        // 生成投射物
        GameObject proj = Object.Instantiate(boss.ProjectilePrefab, spawnPoint.position, Quaternion.identity);

        // 設定投射物速度（投射物應有 Rigidbody2D）
        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb != null)
        {
            projRb.velocity = direction * boss.Data.projectileSpeed;
        }

        Debug.Log("[MiniBoss] 發射投射物！");
    }
}
