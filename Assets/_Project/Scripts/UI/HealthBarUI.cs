using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 玩家血量 UI：監聽玩家 HP 事件頻道，更新血量顯示。
/// 支援心形圖示陣列或血量條兩種顯示模式。
/// </summary>
public class HealthBarUI : MonoBehaviour
{
    #region 欄位

    [Header("事件頻道")]
    [Tooltip("監聽玩家血量變化的事件頻道")]
    [SerializeField] private IntEventChannel playerHealthChannel;

    [Header("心形圖示模式")]
    [Tooltip("心形圖示陣列（每個代表一格血量）")]
    [SerializeField] private Image[] heartIcons;

    [Tooltip("有血量時的心形圖示 Sprite")]
    [SerializeField] private Sprite heartFullSprite;

    [Tooltip("空血量時的心形圖示 Sprite")]
    [SerializeField] private Sprite heartEmptySprite;

    [Header("血量條模式（二選一）")]
    [Tooltip("血量條 Slider（若使用血量條模式）")]
    [SerializeField] private Slider healthSlider;

    [Header("設定")]
    [Tooltip("玩家最大血量（用於計算百分比）")]
    [SerializeField] private int maxHealth = 5;

    #endregion

    #region Unity 生命週期

    private void OnEnable()
    {
        if (playerHealthChannel != null)
            playerHealthChannel.OnEventRaised += OnHealthChanged;
    }

    private void OnDisable()
    {
        if (playerHealthChannel != null)
            playerHealthChannel.OnEventRaised -= OnHealthChanged;
    }

    private void Start()
    {
        // 初始化為滿血
        OnHealthChanged(maxHealth);
    }

    #endregion

    #region 私有方法

    /// <summary>當血量事件觸發時更新 UI。</summary>
    private void OnHealthChanged(int currentHealth)
    {
        // 更新心形圖示
        if (heartIcons != null && heartIcons.Length > 0)
        {
            for (int i = 0; i < heartIcons.Length; i++)
            {
                if (heartIcons[i] == null) continue;
                bool hasHealth = i < currentHealth;
                heartIcons[i].sprite = hasHealth ? heartFullSprite : heartEmptySprite;
                heartIcons[i].color = hasHealth ? Color.white : new Color(1f, 1f, 1f, 0.3f);
            }
        }

        // 更新血量條
        if (healthSlider != null)
        {
            healthSlider.value = maxHealth > 0 ? (float)currentHealth / maxHealth : 0f;
        }
    }

    #endregion
}
