using UnityEngine;

/// <summary>
/// 迷你 Boss 衝刺攻擊狀態：朝玩家方向快速衝刺，碰牆或超時則停止並短暫硬直。
/// </summary>
public class MiniBossDashAttackState : MiniBossBaseState
{
    private float dashTimer;
    private float dashDirection;
    // 碰牆後的短暫停頓時間
    private float recoverTimer;
    private bool isRecovering;
    private const float RecoverDuration = 0.5f;
    // 快取地面 LayerMask，避免每幀執行字串查詢
    private readonly int groundLayerMask = LayerMask.GetMask("Ground");

    public MiniBossDashAttackState(MiniBossController boss, StateMachine stateMachine)
        : base(boss, stateMachine) { }

    public override void Enter()
    {
        dashTimer = boss.Data.dashDuration;
        isRecovering = false;

        // 計算朝玩家的方向
        if (boss.PlayerTransform != null)
            dashDirection = boss.PlayerTransform.position.x > boss.transform.position.x ? 1f : -1f;
        else
            dashDirection = boss.transform.localScale.x > 0 ? 1f : -1f;

        boss.Rb.velocity = new Vector2(dashDirection * boss.Data.dashSpeed, 0f);
    }

    public override void LogicUpdate()
    {
        if (isRecovering)
        {
            recoverTimer -= Time.deltaTime;
            if (recoverTimer <= 0f)
            {
                stateMachine.ChangeState(boss.IdleState);
            }
            return;
        }

        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0f)
        {
            StartRecovery();
        }
    }

    public override void PhysicsUpdate()
    {
        if (isRecovering) return;

        // 碰到牆壁則停止
        bool hitWall = Physics2D.Raycast(
            boss.transform.position,
            new Vector2(dashDirection, 0f),
            1f,
            groundLayerMask
        );

        if (hitWall)
        {
            StartRecovery();
        }
    }

    private void StartRecovery()
    {
        isRecovering = true;
        recoverTimer = RecoverDuration;
        boss.Rb.velocity = Vector2.zero;
    }
}
