using UnityEngine;

/// <summary>
/// Boss 競技場管理器：當玩家進入觸發區域時啟動 Boss 戰，
/// 關閉出入口，Boss 死亡時打開出口。
/// </summary>
public class BossArenaManager : MonoBehaviour
{
    #region 欄位

    [Header("Boss 引用")]
    [Tooltip("此競技場的 Boss 物件")]
    [SerializeField] private GameObject bossObject;

    [Header("入出口")]
    [Tooltip("競技場入口碰撞器（啟動 Boss 戰時關閉）")]
    [SerializeField] private Collider2D arenaEntrance;

    [Tooltip("競技場出口碰撞器（Boss 死亡後開啟）")]
    [SerializeField] private Collider2D arenaExit;

    [Header("事件頻道")]
    [Tooltip("Boss 戰開始事件頻道")]
    [SerializeField] private VoidEventChannel onBossFightStarted;

    [Tooltip("Boss 死亡事件頻道（監聽此頻道來開啟出口）")]
    [SerializeField] private VoidEventChannel onBossDied;

    private bool bossActivated = false;

    #endregion

    #region Unity 生命週期

    private void OnEnable()
    {
        if (onBossDied != null)
            onBossDied.OnEventRaised += OnBossDefeated;
    }

    private void OnDisable()
    {
        if (onBossDied != null)
            onBossDied.OnEventRaised -= OnBossDefeated;
    }

    private void Start()
    {
        // Boss 戰開始前先停用 Boss
        if (bossObject != null)
            bossObject.SetActive(false);

        // 確保出口關閉
        if (arenaExit != null)
            arenaExit.enabled = false;
    }

    /// <summary>當玩家進入觸發區域時啟動 Boss 戰。</summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (bossActivated) return;
        if (!other.CompareTag("Player")) return;

        ActivateBoss();
    }

    #endregion

    #region 私有方法

    /// <summary>啟動 Boss 戰：開啟 Boss、關閉入口、廣播事件。</summary>
    private void ActivateBoss()
    {
        bossActivated = true;

        // 啟動 Boss
        if (bossObject != null)
            bossObject.SetActive(true);

        // 關閉入口（防止玩家逃跑）
        if (arenaEntrance != null)
            arenaEntrance.enabled = true;

        // 通知 GameManager 進入 Boss 戰模式
        GameManager.Instance?.EnterBossFight();

        // 廣播 Boss 戰開始事件
        onBossFightStarted?.RaiseEvent();

        Debug.Log("[BossArenaManager] Boss 戰啟動！");
    }

    /// <summary>Boss 被擊敗後的處理：開啟出口、通知 GameManager。</summary>
    private void OnBossDefeated()
    {
        // 開啟出口
        if (arenaExit != null)
            arenaExit.enabled = true;

        // 關閉入口（已無意義，保持開放）
        if (arenaEntrance != null)
            arenaEntrance.enabled = false;

        // 通知 GameManager 結束 Boss 戰
        GameManager.Instance?.ExitBossFight();

        Debug.Log("[BossArenaManager] Boss 已被擊敗，出口已開啟！");
    }

    #endregion
}
