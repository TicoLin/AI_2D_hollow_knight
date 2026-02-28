using UnityEngine;
using System.Collections;

/// <summary>
/// 迷你 Boss 連斬攻擊狀態：對玩家進行多次連續攻擊，每擊之間有短暫間隔。
/// </summary>
public class MiniBossComboState : MiniBossBaseState
{
    private int comboHitsRemaining;
    private float comboTimer;
    private bool waitingBetweenHits;

    public MiniBossComboState(MiniBossController boss, StateMachine stateMachine)
        : base(boss, stateMachine) { }

    public override void Enter()
    {
        comboHitsRemaining = boss.Data.comboCount;
        comboTimer = 0f;
        waitingBetweenHits = false;

        // 立即執行第一擊
        PerformHit();
    }

    public override void LogicUpdate()
    {
        if (comboHitsRemaining <= 0)
        {
            stateMachine.ChangeState(boss.IdleState);
            return;
        }

        if (waitingBetweenHits)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                waitingBetweenHits = false;
                PerformHit();
            }
        }
    }

    /// <summary>執行一次攻擊。</summary>
    private void PerformHit()
    {
        if (boss.PlayerTransform == null) return;

        float dist = Vector2.Distance(boss.transform.position, boss.PlayerTransform.position);
        if (dist <= boss.Data.dashSpeed * 0.5f) // 使用 attackRange 近似值
        {
            IDamageable damageable = boss.PlayerTransform.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Vector2 dir = (boss.PlayerTransform.position - boss.transform.position).normalized;
                damageable.TakeDamage(boss.Data.comboDamage, dir);
            }
        }

        comboHitsRemaining--;

        if (comboHitsRemaining > 0)
        {
            waitingBetweenHits = true;
            comboTimer = boss.Data.comboPauseDuration;
        }
    }
}
