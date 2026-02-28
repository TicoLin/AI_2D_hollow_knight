using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Boss 血量 UI：監聽 Boss HP 事件頻道，Boss 戰開始時顯示，結束時隱藏。
/// </summary>
public class BossHealthBarUI : MonoBehaviour
{
    #region 欄位

    [Header("事件頻道")]
    [Tooltip("監聽 Boss 血量變化的事件頻道")]
    [SerializeField] private IntEventChannel bossHealthChannel;

    [Tooltip("監聽 Boss 戰開始的事件頻道")]
    [SerializeField] private VoidEventChannel onBossFightStarted;

    [Tooltip("監聽 Boss 死亡的事件頻道")]
    [SerializeField] private VoidEventChannel onBossDied;

    [Header("UI 元件")]
    [Tooltip("Boss 血量條 Slider")]
    [SerializeField] private Slider healthSlider;

    [Tooltip("Boss 名稱文字元件（TextMeshPro）")]
    [SerializeField] private TextMeshProUGUI bossNameText;

    [Tooltip("整個 Boss 血量 UI 根物件（用於顯示/隱藏）")]
    [SerializeField] private GameObject bossHUDRoot;

    [Header("設定")]
    [Tooltip("Boss 最大血量（用於計算百分比）")]
    [SerializeField] private int bossMaxHealth = 300;

    [Tooltip("Boss 名稱（顯示於 UI）")]
    [SerializeField] private string bossDisplayName = "Boss";

    #endregion

    #region Unity 生命週期

    private void Awake()
    {
        // Boss 戰開始前隱藏 UI
        if (bossHUDRoot != null)
            bossHUDRoot.SetActive(false);
    }

    private void OnEnable()
    {
        if (bossHealthChannel != null)
            bossHealthChannel.OnEventRaised += OnBossHealthChanged;
        if (onBossFightStarted != null)
            onBossFightStarted.OnEventRaised += OnBossFightStarted;
        if (onBossDied != null)
            onBossDied.OnEventRaised += OnBossDefeated;
    }

    private void OnDisable()
    {
        if (bossHealthChannel != null)
            bossHealthChannel.OnEventRaised -= OnBossHealthChanged;
        if (onBossFightStarted != null)
            onBossFightStarted.OnEventRaised -= OnBossFightStarted;
        if (onBossDied != null)
            onBossDied.OnEventRaised -= OnBossDefeated;
    }

    #endregion

    #region 私有方法

    /// <summary>Boss 戰開始時顯示 UI。</summary>
    private void OnBossFightStarted()
    {
        if (bossHUDRoot != null)
            bossHUDRoot.SetActive(true);

        if (bossNameText != null)
            bossNameText.text = bossDisplayName;

        if (healthSlider != null)
            healthSlider.value = 1f;
    }

    /// <summary>Boss 血量變化時更新血量條。</summary>
    private void OnBossHealthChanged(int currentHealth)
    {
        if (healthSlider != null && bossMaxHealth > 0)
        {
            healthSlider.value = (float)currentHealth / bossMaxHealth;
        }
    }

    /// <summary>Boss 被擊敗時隱藏 UI。</summary>
    private void OnBossDefeated()
    {
        if (bossHUDRoot != null)
            bossHUDRoot.SetActive(false);
    }

    #endregion
}
