using UnityEngine;

/// <summary>
/// 玩家攻擊狀態：觸發攻擊判定，播放動畫，動畫結束後回到之前的狀態。
/// </summary>
public class PlayerAttackState : PlayerBaseState
{
    private float attackTimer;
    // 攻擊動畫持續時間（與攻擊冷卻相同，可另外從 PlayerData 擴充）
    private const float AttackAnimationDuration = 0.3f;

    public PlayerAttackState(PlayerController controller, StateMachine stateMachine, PlayerData data)
        : base(controller, stateMachine, data) { }

    public override void Enter()
    {
        attackTimer = AttackAnimationDuration;

        // 決定攻擊方向（垂直輸入優先）
        Vector2 attackDirection = GetAttackDirection();

        // 觸發攻擊
        controller.Combat.TryAttack(attackDirection);
        controller.Animator.TriggerAttack();
    }

    public override void LogicUpdate()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            // 攻擊結束：依當前是否在地面決定下個狀態
            stateMachine.ChangeState(controller.IsGrounded
                ? (controller.HorizontalInput != 0f ? (IState)controller.RunState : controller.IdleState)
                : controller.FallState);
        }
    }

    protected override void CheckTransitions()
    {
        // 攻擊動畫期間不允許其他轉移
    }

    /// <summary>根據玩家目前輸入決定攻擊方向。</summary>
    private Vector2 GetAttackDirection()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        if (vertical > 0f)  return Vector2.up;
        if (vertical < 0f)  return Vector2.down;
        return controller.FacingDirection > 0 ? Vector2.right : Vector2.left;
    }
}
