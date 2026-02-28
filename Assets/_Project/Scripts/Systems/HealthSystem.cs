using UnityEngine;

/// <summary>
/// 可受傷害的介面：所有可被攻擊的物件（玩家、敵人、Boss）皆需實作此介面。
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// 接受傷害。
    /// </summary>
    /// <param name="damage">傷害量</param>
    /// <param name="knockbackDirection">擊退方向（已正規化的向量）</param>
    void TakeDamage(int damage, Vector2 knockbackDirection);

    /// <summary>
    /// 死亡處理。
    /// </summary>
    void Die();
}
