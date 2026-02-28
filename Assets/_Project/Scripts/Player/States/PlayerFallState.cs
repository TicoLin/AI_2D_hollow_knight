using UnityEngine;

/// <summary>
/// 玩家下落狀態：在空中向下移動，監聽著地和碰牆事件。
/// </summary>
public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerController controller, StateMachine stateMachine, PlayerData data)
        : base(controller, stateMachine, data) { }

    public override void Enter()
    {
        controller.Animator.UpdateGrounded(false);
    }

    public override void PhysicsUpdate()
    {
        // 空中水平移動
        float targetVelocityX = controller.HorizontalInput * data.moveSpeed;
        float newVelocityX = Mathf.MoveTowards(controller.Rb.velocity.x, targetVelocityX, data.acceleration * Time.fixedDeltaTime);
        controller.Rb.velocity = new Vector2(newVelocityX, controller.Rb.velocity.y);

        if (controller.HorizontalInput != 0f)
            controller.Flip(controller.HorizontalInput);
    }

    protected override void CheckTransitions()
    {
        if (TryDash()) return;
        if (TryAttack()) return;

        // 二段跳
        if (controller.JumpBufferCounter > 0f && controller.RemainingExtraJumps > 0)
        {
            stateMachine.ChangeState(controller.JumpState);
            return;
        }

        // 著地 → 待機或奔跑
        if (controller.IsGrounded)
        {
            controller.ResetExtraJumps();
            stateMachine.ChangeState(controller.HorizontalInput != 0f
                ? (IState)controller.RunState
                : controller.IdleState);
            return;
        }

        // 貼牆 → 滑牆
        if (controller.IsTouchingWall)
        {
            stateMachine.ChangeState(controller.WallSlideState);
        }
    }
}
