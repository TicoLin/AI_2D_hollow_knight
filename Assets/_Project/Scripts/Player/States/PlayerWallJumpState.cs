using UnityEngine;

/// <summary>
/// 玩家牆跳狀態：Enter 時施加反方向力，持續 wallJumpDuration 後轉為下落。
/// </summary>
public class PlayerWallJumpState : PlayerBaseState
{
    private float wallJumpTimer;

    public PlayerWallJumpState(PlayerController controller, StateMachine stateMachine, PlayerData data)
        : base(controller, stateMachine, data) { }

    public override void Enter()
    {
        wallJumpTimer = data.wallJumpDuration;

        // 判斷牆跳方向（從貼牆方向的反方向跳出）
        float jumpDirection = controller.IsTouchingWallRight ? -1f : 1f;
        controller.Flip(jumpDirection);

        // 施加牆跳力
        controller.Rb.velocity = new Vector2(
            jumpDirection * data.wallJumpForce.x,
            data.wallJumpForce.y
        );
    }

    public override void LogicUpdate()
    {
        wallJumpTimer -= Time.deltaTime;
        if (wallJumpTimer <= 0f)
        {
            stateMachine.ChangeState(controller.FallState);
        }
    }

    protected override void CheckTransitions()
    {
        // 牆跳期間不允許其他轉移（強制持續 wallJumpDuration）
    }
}
