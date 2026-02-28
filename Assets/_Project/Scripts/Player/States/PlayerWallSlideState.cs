using UnityEngine;

/// <summary>
/// 玩家滑牆狀態：限制下滑速度，等待跳躍或離牆輸入。
/// </summary>
public class PlayerWallSlideState : PlayerBaseState
{
    public PlayerWallSlideState(PlayerController controller, StateMachine stateMachine, PlayerData data)
        : base(controller, stateMachine, data) { }

    public override void Enter()
    {
        // 重置二段跳次數（滑牆可重置）
        controller.ResetExtraJumps();
        controller.Animator.UpdateWallSlide(true);
    }

    public override void Exit()
    {
        controller.Animator.UpdateWallSlide(false);
    }

    public override void PhysicsUpdate()
    {
        // 限制下滑速度
        if (controller.Rb.velocity.y < -data.wallSlideSpeed)
        {
            controller.Rb.velocity = new Vector2(controller.Rb.velocity.x, -data.wallSlideSpeed);
        }
    }

    protected override void CheckTransitions()
    {
        // 按跳 → 牆跳
        if (controller.JumpInputDown)
        {
            controller.UseJumpBuffer();
            stateMachine.ChangeState(controller.WallJumpState);
            return;
        }

        // 放開方向鍵（不再貼牆） → 下落
        if (!controller.IsTouchingWall)
        {
            stateMachine.ChangeState(controller.FallState);
            return;
        }

        // 著地 → 待機
        if (controller.IsGrounded)
        {
            stateMachine.ChangeState(controller.IdleState);
        }
    }
}
