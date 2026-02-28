using UnityEngine;

/// <summary>
/// 玩家戰鬥系統：處理四方向攻擊判定、冷卻、Pogo 機制。
/// </summary>
public class PlayerCombat : MonoBehaviour
{
    #region 欄位

    [Header("資料")]
    [Tooltip("玩家數值配置")]
    [SerializeField] private PlayerData playerData;

    [Header("攻擊判定點")]
    [Tooltip("右方攻擊判定中心（相對位置）")]
    [SerializeField] private Transform rightAttackPoint;
    [Tooltip("左方攻擊判定中心（相對位置）")]
    [SerializeField] private Transform leftAttackPoint;
    [Tooltip("上方攻擊判定中心（相對位置）")]
    [SerializeField] private Transform upAttackPoint;
    [Tooltip("下方攻擊判定中心（相對位置）")]
    [SerializeField] private Transform downAttackPoint;

    [Header("偵測層")]
    [Tooltip("可被攻擊的目標層")]
    [SerializeField] private LayerMask enemyLayer;

    private Rigidbody2D rb;
    private float lastAttackTime = -999f;

    #endregion

    #region 屬性

    /// <summary>上次攻擊的方向（用於動畫和狀態判斷）。</summary>
    public Vector2 LastAttackDirection { get; private set; } = Vector2.right;

    /// <summary>此次攻擊是否已觸發 Pogo（向下攻擊命中）。</summary>
    public bool PogoTriggered { get; private set; }

    #endregion

    #region Unity 生命週期

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    #endregion

    #region 公開方法

    /// <summary>
    /// 嘗試執行攻擊，若在冷卻中則忽略。
    /// </summary>
    /// <param name="direction">攻擊方向（上/下/左/右）</param>
    /// <returns>是否成功攻擊</returns>
    public bool TryAttack(Vector2 direction)
    {
        if (Time.time < lastAttackTime + playerData.attackCooldown) return false;

        lastAttackTime = Time.time;
        LastAttackDirection = direction;
        PogoTriggered = false;

        PerformAttack(direction);
        return true;
    }

    #endregion

    #region 私有方法

    /// <summary>根據方向執行攻擊判定。</summary>
    private void PerformAttack(Vector2 direction)
    {
        Transform attackPoint = GetAttackPoint(direction);
        if (attackPoint == null) return;

        // 使用 OverlapCircle 偵測範圍內的可傷害目標
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            playerData.attackRange,
            enemyLayer
        );

        foreach (Collider2D hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // 擊退方向為攻擊方向
                damageable.TakeDamage(playerData.attackDamage, direction);

                // Pogo：向下攻擊命中時觸發
                if (direction == Vector2.down)
                {
                    PogoTriggered = true;
                    rb.velocity = new Vector2(rb.velocity.x, playerData.pogoForce);
                }
            }
        }
    }

    /// <summary>根據攻擊方向取得對應攻擊點。</summary>
    private Transform GetAttackPoint(Vector2 direction)
    {
        if (direction == Vector2.right)  return rightAttackPoint;
        if (direction == Vector2.left)   return leftAttackPoint;
        if (direction == Vector2.up)     return upAttackPoint;
        if (direction == Vector2.down)   return downAttackPoint;
        return rightAttackPoint; // 預設
    }

    #endregion

    #region Gizmos 除錯

    private void OnDrawGizmosSelected()
    {
        if (playerData == null) return;
        Gizmos.color = Color.red;
        if (rightAttackPoint != null)
            Gizmos.DrawWireSphere(rightAttackPoint.position, playerData.attackRange);
        if (leftAttackPoint != null)
            Gizmos.DrawWireSphere(leftAttackPoint.position, playerData.attackRange);
        if (upAttackPoint != null)
            Gizmos.DrawWireSphere(upAttackPoint.position, playerData.attackRange);
        if (downAttackPoint != null)
            Gizmos.DrawWireSphere(downAttackPoint.position, playerData.attackRange);
    }

    #endregion
}
