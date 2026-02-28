using UnityEngine;

/// <summary>
/// 玩家受傷狀態：短暫硬直 + 擊退，結束後依當前情況回到待機或下落狀態。
/// </summary>
public class PlayerHurtState : PlayerBaseState
{
    private float hurtTimer;
    // 受傷硬直時間（秒）
    private const float HurtDuration = 0.3f;

    public PlayerHurtState(PlayerController controller, StateMachine stateMachine, PlayerData data)
        : base(controller, stateMachine, data) { }

    public override void Enter()
    {
        hurtTimer = HurtDuration;
        controller.Animator.TriggerHurt();
    }

    public override void LogicUpdate()
    {
        hurtTimer -= Time.deltaTime;
        if (hurtTimer <= 0f)
        {
            // 依是否在地面回到適當狀態
            stateMachine.ChangeState(controller.IsGrounded
                ? (IState)controller.IdleState
                : controller.FallState);
        }
    }

    protected override void CheckTransitions()
    {
        // 受傷硬直期間不允許其他轉移
    }
}
