using UnityEngine;
using System.Collections;

/// <summary>
/// 畫面凍結（Hit Stop）效果：短暫將 Time.timeScale 設為 0 再恢復，
/// 增強攻擊的打擊感。使用靜態方法，可從任何地方呼叫。
/// </summary>
public class HitStop : MonoBehaviour
{
    #region 欄位

    private static HitStop instance;

    #endregion

    #region Unity 生命週期

    private void Awake()
    {
        // 簡易單例（不繼承 Singleton，避免 DontDestroyOnLoad 複雜度）
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    #endregion

    #region 公開靜態方法

    /// <summary>
    /// 執行畫面凍結效果。
    /// </summary>
    /// <param name="duration">凍結持續時間（秒，使用非縮放時間）</param>
    public static void Freeze(float duration)
    {
        if (instance == null)
        {
            Debug.LogWarning("[HitStop] 場景中沒有 HitStop 組件！");
            return;
        }
        instance.StartCoroutine(instance.FreezeCoroutine(duration));
    }

    #endregion

    #region 協程

    /// <summary>凍結協程：暫停 timeScale 後以 WaitForSecondsRealtime 等待後恢復。</summary>
    private IEnumerator FreezeCoroutine(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    #endregion
}
