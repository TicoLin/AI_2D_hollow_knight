using UnityEngine;

/// <summary>
/// 玩家血量管理：實作 IDamageable，處理受傷、無敵幀、擊退、死亡，並廣播血量變化事件。
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamageable
{
    #region 欄位

    [Header("資料")]
    [Tooltip("玩家數值配置 ScriptableObject")]
    [SerializeField] private PlayerData playerData;

    [Header("事件頻道")]
    [Tooltip("血量變化時廣播的事件頻道（攜帶目前血量）")]
    [SerializeField] private IntEventChannel onHealthChanged;

    [Tooltip("玩家死亡時廣播的事件頻道")]
    [SerializeField] private VoidEventChannel onPlayerDied;

    private Rigidbody2D rb;
    private PlayerController playerController;

    #endregion

    #region 屬性

    /// <summary>目前血量。</summary>
    public int CurrentHealth { get; private set; }

    /// <summary>是否處於無敵幀中。</summary>
    public bool IsInvincible { get; private set; }

    #endregion

    #region Unity 生命週期

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        CurrentHealth = playerData.maxHealth;
    }

    #endregion

    #region IDamageable 實作

    /// <summary>
    /// 接受傷害，套用無敵幀與擊退效果。
    /// </summary>
    /// <param name="damage">傷害量</param>
    /// <param name="knockbackDirection">擊退方向（已正規化）</param>
    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        if (IsInvincible) return;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Max(CurrentHealth, 0);

        // 廣播血量變化
        onHealthChanged?.RaiseEvent(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die();
            return;
        }

        // 施加擊退力
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackDirection.normalized * playerData.hurtKnockbackForce, ForceMode2D.Impulse);

        // 通知控制器進入受傷狀態
        playerController?.OnHurt(knockbackDirection);

        // 啟動無敵幀
        StartCoroutine(InvincibilityCoroutine());
    }

    /// <summary>
    /// 玩家死亡處理。
    /// </summary>
    public void Die()
    {
        onPlayerDied?.RaiseEvent();
        Debug.Log("[PlayerHealth] 玩家死亡！");
        // TODO：播放死亡動畫、觸發場景重置等
        gameObject.SetActive(false);
    }

    #endregion

    #region 私有方法

    /// <summary>無敵幀協程：在指定時間後解除無敵。</summary>
    private System.Collections.IEnumerator InvincibilityCoroutine()
    {
        IsInvincible = true;
        yield return new WaitForSeconds(playerData.invincibilityDuration);
        IsInvincible = false;
    }

    #endregion

    #region 公開方法

    /// <summary>
    /// 治療玩家（增加血量，不超過上限）。
    /// </summary>
    /// <param name="amount">治療量</param>
    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, playerData.maxHealth);
        onHealthChanged?.RaiseEvent(CurrentHealth);
    }

    #endregion
}
