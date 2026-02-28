using UnityEngine;

/// <summary>
/// 視差背景系統：根據相機移動，以不同速率移動各背景圖層，製造景深效果。
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
    #region 巢狀結構

    [System.Serializable]
    public struct ParallaxLayer
    {
        [Tooltip("背景圖層的 Transform")]
        public Transform layerTransform;

        [Tooltip("視差因子（0 = 完全跟隨相機，1 = 完全不動）")]
        [Range(0f, 1f)]
        public float parallaxFactor;
    }

    #endregion

    #region 欄位

    [Header("相機")]
    [Tooltip("作為視差計算基準的相機")]
    [SerializeField] private Camera mainCamera;

    [Header("視差圖層")]
    [Tooltip("各視差背景圖層設定")]
    [SerializeField] private ParallaxLayer[] layers;

    // 上一幀的相機位置（用於計算位移差）
    private Vector3 lastCameraPosition;

    #endregion

    #region Unity 生命週期

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        lastCameraPosition = mainCamera.transform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = mainCamera.transform.position - lastCameraPosition;

        foreach (ParallaxLayer layer in layers)
        {
            if (layer.layerTransform == null) continue;

            // 視差偏移 = 相機移動量 × (1 - 視差因子)
            float parallaxX = deltaMovement.x * (1f - layer.parallaxFactor);
            float parallaxY = deltaMovement.y * (1f - layer.parallaxFactor);

            layer.layerTransform.position += new Vector3(parallaxX, parallaxY, 0f);
        }

        lastCameraPosition = mainCamera.transform.position;
    }

    #endregion
}
