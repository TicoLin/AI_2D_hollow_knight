using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 攜帶整數參數的事件頻道（ScriptableObject）。
/// 用於廣播攜帶整數資料的事件（例如：血量變化）。
/// </summary>
[CreateAssetMenu(menuName = "Events/Int Event Channel", fileName = "IntEventChannel")]
public class IntEventChannel : ScriptableObject
{
    #region 事件

    /// <summary>當事件被觸發時呼叫的委派（攜帶 int 參數）。</summary>
    public UnityAction<int> OnEventRaised;

    #endregion

    #region 公開方法

    /// <summary>
    /// 觸發此事件頻道，通知所有監聽者並傳遞整數值。
    /// </summary>
    /// <param name="value">要廣播的整數值</param>
    public void RaiseEvent(int value)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(value);
        }
        else
        {
            Debug.LogWarning($"[IntEventChannel] {name} 被觸發（值：{value}），但沒有任何監聽者。");
        }
    }

    #endregion
}
