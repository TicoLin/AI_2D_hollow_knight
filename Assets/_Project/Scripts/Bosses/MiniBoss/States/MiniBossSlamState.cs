using UnityEngine;

/// <summary>
/// 迷你 Boss 砸地攻擊狀態：跳至高處後向下砸，著地產生範圍傷害。
/// </summary>
public class MiniBossSlamState : MiniBossBaseState
{
    private enum SlamPhase { Jumping, Falling, Impact }
    private SlamPhase slamPhase;
    private float fallTimer;
    private const float MaxFallTime = 3f;
    // 著地速度閾值
    private const float LandingVelocityThreshold = 0.5f;
    // 快取玩家 LayerMask，避免每次 OnImpact 執行字串查詢
    private readonly int playerLayerMask = LayerMask.GetMask("Player");

    public MiniBossSlamState(MiniBossController boss, StateMachine stateMachine)
        : base(boss, stateMachine) { }

    public override void Enter()
    {
        slamPhase = SlamPhase.Jumping;

        // 跳躍至高處（朝玩家上方）
        boss.Rb.velocity = new Vector2(0f, boss.Data.slamJumpForce);
    }

    public override void LogicUpdate()
    {
        switch (slamPhase)
        {
            case SlamPhase.Jumping:
                // 到達頂點後開始下落
                if (boss.Rb.velocity.y <= 0f)
                {
                    slamPhase = SlamPhase.Falling;
                    fallTimer = MaxFallTime;
                    // 加速下落
                    boss.Rb.gravityScale = 3f;
                }
                break;

            case SlamPhase.Falling:
                fallTimer -= Time.deltaTime;
                // 著地偵測（簡易：檢查 y 速度接近 0 且有碰撞）
                if (Mathf.Abs(boss.Rb.velocity.y) < LandingVelocityThreshold || fallTimer <= 0f)
                {
                    slamPhase = SlamPhase.Impact;
                    OnImpact();
                }
                break;

            case SlamPhase.Impact:
                // 衝擊處理後回到待機
                boss.Rb.gravityScale = 1f;
                stateMachine.ChangeState(boss.IdleState);
                break;
        }
    }

    /// <summary>著地時的範圍傷害。</summary>
    private void OnImpact()
    {
        boss.Rb.velocity = Vector2.zero;

        // 偵測範圍內的玩家並造成傷害
        Collider2D[] hits = Physics2D.OverlapCircleAll(boss.transform.position, boss.Data.slamRadius, playerLayerMask);
        foreach (Collider2D hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Vector2 dir = (hit.transform.position - boss.transform.position).normalized;
                damageable.TakeDamage(boss.Data.contactDamage * 2, dir);
            }
        }

        // TODO：播放砸地特效
        Debug.Log("[MiniBoss] 砸地攻擊！");
    }
}
