using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 遊戲管理器：管理遊戲暫停、遊戲狀態（Playing / Paused / BossFight）。
/// 繼承泛型單例，場景切換時不被銷毀。
/// </summary>
public class GameManager : Singleton<GameManager>
{
    #region 列舉

    /// <summary>遊戲狀態列舉。</summary>
    public enum GameState
    {
        Playing,
        Paused,
        BossFight
    }

    #endregion

    #region 欄位

    [Header("事件頻道")]
    [Tooltip("遊戲暫停時觸發的事件頻道")]
    [SerializeField] private VoidEventChannel onGamePausedChannel;

    [Tooltip("遊戲繼續時觸發的事件頻道")]
    [SerializeField] private VoidEventChannel onGameResumedChannel;

    #endregion

    #region 屬性

    /// <summary>目前的遊戲狀態。</summary>
    public GameState CurrentState { get; private set; } = GameState.Playing;

    /// <summary>遊戲暫停事件。</summary>
    public UnityAction OnGamePaused;

    /// <summary>遊戲繼續事件。</summary>
    public UnityAction OnGameResumed;

    #endregion

    #region 公開方法

    /// <summary>
    /// 暫停遊戲（將 Time.timeScale 設為 0）。
    /// </summary>
    public void PauseGame()
    {
        if (CurrentState == GameState.Paused) return;

        CurrentState = GameState.Paused;
        Time.timeScale = 0f;
        OnGamePaused?.Invoke();
        onGamePausedChannel?.RaiseEvent();
        Debug.Log("[GameManager] 遊戲已暫停");
    }

    /// <summary>
    /// 繼續遊戲（將 Time.timeScale 設回 1）。
    /// </summary>
    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused) return;

        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
        OnGameResumed?.Invoke();
        onGameResumedChannel?.RaiseEvent();
        Debug.Log("[GameManager] 遊戲已繼續");
    }

    /// <summary>
    /// 切換至 Boss 戰模式。
    /// </summary>
    public void EnterBossFight()
    {
        CurrentState = GameState.BossFight;
        Debug.Log("[GameManager] 進入 Boss 戰");
    }

    /// <summary>
    /// 結束 Boss 戰，回到一般遊玩狀態。
    /// </summary>
    public void ExitBossFight()
    {
        CurrentState = GameState.Playing;
        Debug.Log("[GameManager] Boss 戰結束");
    }

    /// <summary>
    /// 切換暫停狀態（暫停 ↔ 繼續）。
    /// </summary>
    public void TogglePause()
    {
        if (CurrentState == GameState.Paused)
            ResumeGame();
        else
            PauseGame();
    }

    #endregion
}
