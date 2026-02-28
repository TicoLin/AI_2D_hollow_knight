using UnityEngine;

/// <summary>
/// 玩家奔跑狀態：水平移動，使用加速/減速曲線。
/// </summary>
public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerController controller, StateMachine stateMachine, PlayerData data)
        : base(controller, stateMachine, data) { }

    public override void Enter()
    {
        controller.Animator.UpdateRunning(true);
        controller.Animator.UpdateGrounded(true);
    }

    public override void PhysicsUpdate()
    {
        float targetVelocityX = controller.HorizontalInput * data.moveSpeed;
        float currentVelocityX = controller.Rb.velocity.x;

        // 根據有無輸入選擇加速或減速
        float accelerationRate = (controller.HorizontalInput != 0f) ? data.acceleration : data.deceleration;

        float newVelocityX = Mathf.MoveTowards(currentVelocityX, targetVelocityX, accelerationRate * Time.fixedDeltaTime);
        controller.Rb.velocity = new Vector2(newVelocityX, controller.Rb.velocity.y);

        // 翻轉角色面向
        if (controller.HorizontalInput != 0f)
            controller.Flip(controller.HorizontalInput);
    }

    protected override void CheckTransitions()
    {
        if (TryDash()) return;
        if (TryAttack()) return;

        // 無輸入 → 待機
        if (controller.HorizontalInput == 0f && Mathf.Abs(controller.Rb.velocity.x) < 0.1f)
        {
            stateMachine.ChangeState(controller.IdleState);
            return;
        }

        // 跳躍（含緩衝）
        if (controller.JumpBufferCounter > 0f)
        {
            stateMachine.ChangeState(controller.JumpState);
            return;
        }

        // 離地 → 下落
        if (!controller.IsGrounded && controller.CoyoteTimeCounter <= 0f)
        {
            stateMachine.ChangeState(controller.FallState);
        }
    }
}
