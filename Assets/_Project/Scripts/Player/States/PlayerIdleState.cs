/// <summary>
/// 玩家待機狀態：站立不動時的狀態，監聽輸入以切換至其他狀態。
/// </summary>
public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController controller, StateMachine stateMachine, PlayerData data)
        : base(controller, stateMachine, data) { }

    public override void Enter()
    {
        // 停止水平移動
        controller.Rb.velocity = new UnityEngine.Vector2(0f, controller.Rb.velocity.y);
        controller.Animator.UpdateRunning(false);
        controller.Animator.UpdateGrounded(true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    protected override void CheckTransitions()
    {
        // 衝刺優先
        if (TryDash()) return;

        // 攻擊
        if (TryAttack()) return;

        // 有水平輸入 → 奔跑
        if (controller.HorizontalInput != 0f)
        {
            stateMachine.ChangeState(controller.RunState);
            return;
        }

        // 跳躍（含跳躍緩衝）
        if (controller.JumpBufferCounter > 0f)
        {
            stateMachine.ChangeState(controller.JumpState);
            return;
        }

        // 離開地面（被推落）→ 下落
        if (!controller.IsGrounded && controller.CoyoteTimeCounter <= 0f)
        {
            stateMachine.ChangeState(controller.FallState);
        }
    }
}
