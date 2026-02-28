using UnityEngine;

/// <summary>
/// 玩家衝刺狀態：Enter 時設定速度，持續 dashDuration，期間無重力，結束後回到適當狀態。
/// </summary>
public class PlayerDashState : PlayerBaseState
{
    private float dashTimer;
    private float originalGravityScale;

    public PlayerDashState(PlayerController controller, StateMachine stateMachine, PlayerData data)
        : base(controller, stateMachine, data) { }

    public override void Enter()
    {
        controller.UseDash();
        dashTimer = data.dashDuration;

        // 暫停重力
        originalGravityScale = controller.Rb.gravityScale;
        controller.Rb.gravityScale = 0f;

        // 設定衝刺速度（依面朝方向）
        float dashDirection = controller.FacingDirection;
        controller.Rb.velocity = new Vector2(dashDirection * data.dashSpeed, 0f);

        controller.Animator.TriggerDash();
    }

    public override void Exit()
    {
        // 恢復重力
        controller.Rb.gravityScale = originalGravityScale;
    }

    public override void LogicUpdate()
    {
        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0f)
        {
            // 衝刺結束：依是否在地面決定下個狀態
            stateMachine.ChangeState(controller.IsGrounded
                ? (IState)controller.IdleState
                : controller.FallState);
        }
    }

    protected override void CheckTransitions()
    {
        // 衝刺期間不允許其他轉移
    }
}
