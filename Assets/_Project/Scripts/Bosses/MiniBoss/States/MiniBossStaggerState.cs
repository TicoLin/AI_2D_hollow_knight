using UnityEngine;

/// <summary>
/// 迷你 Boss 硬直狀態：受到一定傷害後進入硬直，給予玩家攻擊窗口。
/// </summary>
public class MiniBossStaggerState : MiniBossBaseState
{
    private float staggerTimer;

    public MiniBossStaggerState(MiniBossController boss, StateMachine stateMachine)
        : base(boss, stateMachine) { }

    public override void Enter()
    {
        staggerTimer = boss.Data.staggerDuration;
        boss.Rb.velocity = Vector2.zero;

        Debug.Log("[MiniBoss] 進入硬直狀態！（玩家攻擊窗口）");
        // TODO：播放硬直動畫（閃爍、特效等）
    }

    public override void LogicUpdate()
    {
        staggerTimer -= Time.deltaTime;
        if (staggerTimer <= 0f)
        {
            // 硬直結束，回到待機並選擇下一招
            stateMachine.ChangeState(boss.IdleState);
        }
    }
}
