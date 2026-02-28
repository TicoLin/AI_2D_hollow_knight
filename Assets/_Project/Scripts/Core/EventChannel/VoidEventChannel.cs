using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 無參數事件頻道（ScriptableObject）。
/// 用於廣播不攜帶資料的事件，遵循觀察者模式。
/// </summary>
[CreateAssetMenu(menuName = "Events/Void Event Channel", fileName = "VoidEventChannel")]
public class VoidEventChannel : ScriptableObject
{
    #region 事件

    /// <summary>當事件被觸發時呼叫的委派。</summary>
    public UnityAction OnEventRaised;

    #endregion

    #region 公開方法

    /// <summary>
    /// 觸發此事件頻道，通知所有監聽者。
    /// </summary>
    public void RaiseEvent()
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke();
        }
        else
        {
            Debug.LogWarning($"[VoidEventChannel] {name} 被觸發，但沒有任何監聽者。");
        }
    }

    #endregion
}
