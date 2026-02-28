using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 無參數事件監聽器組件。
/// 在 Inspector 中指定要監聽的 VoidEventChannel 與對應的回應事件。
/// </summary>
public class VoidEventListener : MonoBehaviour
{
    #region 欄位

    [Tooltip("要監聽的無參數事件頻道")]
    [SerializeField] private VoidEventChannel eventChannel;

    [Tooltip("事件被觸發時要執行的 UnityEvent")]
    [SerializeField] private UnityEvent response;

    #endregion

    #region Unity 生命週期

    private void OnEnable()
    {
        if (eventChannel != null)
        {
            eventChannel.OnEventRaised += OnEventRaised;
        }
    }

    private void OnDisable()
    {
        if (eventChannel != null)
        {
            eventChannel.OnEventRaised -= OnEventRaised;
        }
    }

    #endregion

    #region 私有方法

    /// <summary>當事件頻道觸發時執行 response。</summary>
    private void OnEventRaised()
    {
        response?.Invoke();
    }

    #endregion
}
