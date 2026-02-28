using UnityEngine;

/// <summary>
/// 敵人數據 ScriptableObject：存放所有可在 Inspector 中調整的敵人數值配置。
/// </summary>
[CreateAssetMenu(menuName = "Data/Enemy Data", fileName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    #region 移動

    [Header("移動")]
    [Tooltip("巡邏移動速度")]
    public float moveSpeed = 2f;

    [Tooltip("追擊移動速度")]
    public float chaseSpeed = 4f;

    [Tooltip("巡邏折返等待時間（秒）")]
    public float patrolWaitTime = 1f;

    #endregion

    #region 戰鬥

    [Header("戰鬥")]
    [Tooltip("攻擊傷害")]
    public int attackDamage = 1;

    [Tooltip("攻擊範圍")]
    public float attackRange = 1f;

    [Tooltip("攻擊冷卻時間（秒）")]
    public float attackCooldown = 1.5f;

    #endregion

    #region 偵測

    [Header("偵測")]
    [Tooltip("偵測玩家的範圍")]
    public float detectionRange = 6f;

    [Tooltip("失去目標後停止追擊的範圍")]
    public float loseTargetRange = 10f;

    #endregion

    #region 血量

    [Header("血量")]
    [Tooltip("最大血量")]
    public int maxHealth = 3;

    [Tooltip("受傷擊退力道")]
    public float hurtKnockbackForce = 5f;

    [Tooltip("受傷硬直時間（秒）")]
    public float hurtDuration = 0.3f;

    #endregion
}
