using UnityEngine;

/// <summary>
/// 玩家數據 ScriptableObject：存放所有可在 Inspector 中調整的玩家數值配置。
/// </summary>
[CreateAssetMenu(menuName = "Data/Player Data", fileName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    #region 移動

    [Header("移動")]
    [Tooltip("最高水平移動速度")]
    public float moveSpeed = 8f;

    [Tooltip("地面加速度")]
    public float acceleration = 10f;

    [Tooltip("地面減速度")]
    public float deceleration = 15f;

    #endregion

    #region 跳躍

    [Header("跳躍")]
    [Tooltip("跳躍初速度")]
    public float jumpForce = 14f;

    [Tooltip("放開跳躍鍵時垂直速度乘數（可變高度跳躍）")]
    public float variableJumpMultiplier = 0.5f;

    [Tooltip("土狼時間：離地後仍可跳躍的時間（秒）")]
    public float coyoteTime = 0.1f;

    [Tooltip("跳躍緩衝：落地前按跳躍鍵可預儲跳躍的時間（秒）")]
    public float jumpBufferTime = 0.15f;

    [Tooltip("額外跳躍次數（0 = 無二段跳，1 = 有一次二段跳）")]
    public int extraJumpCount = 1;

    #endregion

    #region 滑牆 / 牆跳

    [Header("滑牆 / 牆跳")]
    [Tooltip("滑牆時最大下滑速度")]
    public float wallSlideSpeed = 2f;

    [Tooltip("牆跳時施加的力（x：水平，y：垂直）")]
    public Vector2 wallJumpForce = new Vector2(12f, 14f);

    [Tooltip("牆跳後玩家被強制移動的持續時間（秒）")]
    public float wallJumpDuration = 0.2f;

    #endregion

    #region 衝刺

    [Header("衝刺")]
    [Tooltip("衝刺速度")]
    public float dashSpeed = 20f;

    [Tooltip("衝刺持續時間（秒）")]
    public float dashDuration = 0.15f;

    [Tooltip("衝刺冷卻時間（秒）")]
    public float dashCooldown = 0.5f;

    #endregion

    #region 戰鬥

    [Header("戰鬥")]
    [Tooltip("攻擊判定範圍半徑")]
    public float attackRange = 1.2f;

    [Tooltip("每次攻擊造成的傷害值")]
    public int attackDamage = 1;

    [Tooltip("攻擊冷卻時間（秒）")]
    public float attackCooldown = 0.3f;

    [Tooltip("向下攻擊命中時給予玩家的向上彈力（Pogo 機制）")]
    public float pogoForce = 10f;

    #endregion

    #region 血量

    [Header("血量")]
    [Tooltip("玩家最大血量")]
    public int maxHealth = 5;

    [Tooltip("受傷後的無敵幀持續時間（秒）")]
    public float invincibilityDuration = 1f;

    [Tooltip("受傷擊退力道")]
    public float hurtKnockbackForce = 8f;

    #endregion

    #region 物理偵測

    [Header("物理偵測")]
    [Tooltip("地面偵測 BoxCast 的大小")]
    public Vector2 groundCheckSize = new Vector2(0.8f, 0.05f);

    [Tooltip("牆壁偵測 BoxCast 的大小")]
    public Vector2 wallCheckSize = new Vector2(0.05f, 0.8f);

    [Tooltip("地面偵測距離")]
    public float groundCheckDistance = 0.1f;

    [Tooltip("牆壁偵測距離")]
    public float wallCheckDistance = 0.1f;

    [Tooltip("地面層 LayerMask")]
    public LayerMask groundLayer;

    [Tooltip("牆壁層 LayerMask")]
    public LayerMask wallLayer;

    #endregion
}
