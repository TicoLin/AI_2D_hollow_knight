using UnityEngine;

/// <summary>
/// Boss 數據 ScriptableObject：存放所有可在 Inspector 中調整的 Boss 數值配置。
/// </summary>
[CreateAssetMenu(menuName = "Data/Boss Data", fileName = "BossData")]
public class BossData : ScriptableObject
{
    #region 基礎

    [Header("基礎")]
    [Tooltip("Boss 最大血量")]
    public int maxHealth = 300;

    [Tooltip("接觸傷害")]
    public int contactDamage = 1;

    [Tooltip("Boss 名稱（用於 UI 顯示）")]
    public string bossName = "Boss";

    #endregion

    #region 衝刺攻擊

    [Header("衝刺攻擊")]
    [Tooltip("衝刺速度")]
    public float dashSpeed = 18f;

    [Tooltip("衝刺持續時間（秒）")]
    public float dashDuration = 0.4f;

    [Tooltip("衝刺攻擊冷卻時間（秒）")]
    public float dashCooldown = 3f;

    #endregion

    #region 砸地攻擊

    [Header("砸地攻擊")]
    [Tooltip("砸地跳躍力")]
    public float slamJumpForce = 20f;

    [Tooltip("砸地傷害範圍半徑")]
    public float slamRadius = 3f;

    [Tooltip("砸地攻擊冷卻時間（秒）")]
    public float slamCooldown = 4f;

    #endregion

    #region 連斬攻擊

    [Header("連斬攻擊")]
    [Tooltip("連斬次數")]
    public int comboCount = 3;

    [Tooltip("每次連斬之間的間隔（秒）")]
    public float comboPauseDuration = 0.3f;

    [Tooltip("連斬攻擊冷卻時間（秒）")]
    public float comboCooldown = 4f;

    [Tooltip("連斬每擊造成的傷害")]
    public int comboDamage = 1;

    #endregion

    #region 投射物攻擊（Phase 2）

    [Header("投射物攻擊（Phase 2）")]
    [Tooltip("投射物速度")]
    public float projectileSpeed = 10f;

    [Tooltip("投射物傷害")]
    public int projectileDamage = 2;

    [Tooltip("投射物攻擊冷卻時間（秒）")]
    public float projectileCooldown = 3f;

    #endregion

    #region 硬直

    [Header("硬直")]
    [Tooltip("每累積此傷害後進入硬直狀態")]
    public int staggerThreshold = 50;

    [Tooltip("硬直持續時間（秒）")]
    public float staggerDuration = 2f;

    #endregion

    #region 階段

    [Header("階段")]
    [Tooltip("Phase 切換的 HP 百分比閾值（例如 0.5 = 50% HP 時進入 Phase 2）")]
    public float[] phaseThresholds = { 0.5f };

    #endregion
}
