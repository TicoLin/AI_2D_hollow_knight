using UnityEngine;
using System.Collections;

/// <summary>
/// 相機系統：平滑跟隨玩家，支援螢幕震動功能。
/// </summary>
public class CameraSystem : MonoBehaviour
{
    #region 欄位

    [Header("跟隨目標")]
    [Tooltip("相機跟隨的目標（通常是玩家）")]
    [SerializeField] private Transform target;

    [Header("跟隨設定")]
    [Tooltip("跟隨偏移量（相對於目標）")]
    [SerializeField] private Vector3 followOffset = new Vector3(0f, 1f, -10f);

    [Tooltip("相機平滑速度")]
    [SerializeField] private float smoothSpeed = 5f;

    // SmoothDamp 參考速度
    private Vector3 velocity = Vector3.zero;

    // 震動相關
    private bool isShaking;
    private Vector3 originalLocalPosition;

    #endregion

    #region Unity 生命週期

    private void LateUpdate()
    {
        if (target == null) return;
        FollowTarget();
    }

    #endregion

    #region 私有方法

    /// <summary>平滑跟隨目標位置。</summary>
    private void FollowTarget()
    {
        Vector3 desiredPosition = target.position + followOffset;
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            1f / smoothSpeed
        );
    }

    #endregion

    #region 公開方法

    /// <summary>
    /// 執行螢幕震動效果。
    /// </summary>
    /// <param name="duration">震動持續時間（秒）</param>
    /// <param name="magnitude">震動幅度</param>
    public void ShakeCamera(float duration, float magnitude)
    {
        if (isShaking) StopAllCoroutines();
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    /// <summary>
    /// 設定跟隨目標。
    /// </summary>
    /// <param name="newTarget">新的跟隨目標</param>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    #endregion

    #region 協程

    /// <summary>螢幕震動協程。</summary>
    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        isShaking = true;
        originalLocalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalLocalPosition + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalLocalPosition;
        isShaking = false;
    }

    #endregion
}
