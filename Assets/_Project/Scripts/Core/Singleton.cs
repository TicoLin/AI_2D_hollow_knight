using UnityEngine;

/// <summary>
/// 泛型單例基底類別，確保場景中只有一個實例，並在場景切換時不被銷毀。
/// </summary>
/// <typeparam name="T">繼承 MonoBehaviour 的類別型別</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region 欄位

    private static T _instance;

    [Tooltip("是否在載入新場景時保留此物件（DontDestroyOnLoad）")]
    [SerializeField] private bool persistAcrossScenes = true;

    #endregion

    #region 屬性

    /// <summary>取得單例實例。</summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    Debug.LogError($"[Singleton] 場景中找不到 {typeof(T).Name} 的實例！");
                }
            }
            return _instance;
        }
    }

    #endregion

    #region Unity 生命週期

    protected virtual void Awake()
    {
        // 防止重複實例
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;

        if (persistAcrossScenes)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion
}
