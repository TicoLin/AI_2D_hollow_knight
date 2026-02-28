using UnityEngine;

/// <summary>
/// 玩家跳躍狀態：施加跳躍力，支援可變高度跳躍（放開鍵提前降速）。
/// </summary>
public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController controller, StateMachine stateMachine, PlayerData data)
        : base(controller, stateMachine, data) { }

    public override void Enter()
    {
        // 消耗跳躍緩衝
        controller.UseJumpBuffer();

        // 判斷是否為二段跳
        bool isExtraJump = !controller.IsGrounded && controller.CoyoteTimeCounter <= 0f;
        if (isExtraJump)
        {
            controller.UseExtraJump();
        }
        else
        {
            controller.ResetExtraJumps();
        }

        // 施加跳躍力
        controller.Rb.velocity = new Vector2(controller.Rb.velocity.x, data.jumpForce);
        controller.Animator.UpdateGrounded(false);
    }

    public override void PhysicsUpdate()
    {
        // 水平移動保持
        float targetVelocityX = controller.HorizontalInput * data.moveSpeed;
        float newVelocityX = Mathf.MoveTowards(controller.Rb.velocity.x, targetVelocityX, data.acceleration * Time.fixedDeltaTime);
        controller.Rb.velocity = new Vector2(newVelocityX, controller.Rb.velocity.y);

        // 可變高度跳躍：放開跳躍鍵時減少垂直速度
        if (!controller.JumpInputHeld && controller.Rb.velocity.y > 0f)
        {
            controller.Rb.velocity = new Vector2(
                controller.Rb.velocity.x,
                controller.Rb.velocity.y * data.variableJumpMultiplier
            );
        }

        if (controller.HorizontalInput != 0f)
            controller.Flip(controller.HorizontalInput);
    }

    protected override void CheckTransitions()
    {
        if (TryDash()) return;
        if (TryAttack()) return;

        // 貼牆 → 滑牆
        if (controller.IsTouchingWall)
        {
            stateMachine.ChangeState(controller.WallSlideState);
            return;
        }

        // 速度轉為向下 → 下落
        if (controller.Rb.velocity.y < 0f)
        {
            stateMachine.ChangeState(controller.FallState);
        }
    }
}
