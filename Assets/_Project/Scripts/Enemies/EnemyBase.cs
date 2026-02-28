using UnityEngine;

/// <summary>
/// 敵人基底類別：實作 IDamageable，管理敵人狀態機、偵測玩家、受傷與死亡。
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyBase : MonoBehaviour, IDamageable
{
    #region 欄位

    [Header("資料")]
    [Tooltip("敵人數值配置")]
    [SerializeField] protected EnemyData enemyData;

    [Header("巡邏點")]
    [Tooltip("巡邏的左邊界點")]
    [SerializeField] protected Transform patrolPointLeft;
    [Tooltip("巡邏的右邊界點")]
    [SerializeField] protected Transform patrolPointRight;

    [Header("偵測層")]
    [Tooltip("玩家所在的層")]
    [SerializeField] protected LayerMask playerLayer;

    // 組件引用
    protected StateMachine stateMachine;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Transform playerTransform;

    // 狀態引用
    protected EnemyIdleState idleState;
    protected EnemyChaseState chaseState;
    protected EnemyAttackState attackState;
    protected EnemyHurtState hurtState;

    // 血量
    private int currentHealth;

    #endregion

    #region 屬性

    /// <summary>敵人數值配置。</summary>
    public EnemyData Data => enemyData;

    /// <summary>Rigidbody2D 引用。</summary>
    public Rigidbody2D Rb => rb;

    /// <summary>Animator 引用。</summary>
    public Animator Anim => animator;

    /// <summary>玩家 Transform 引用（可能為 null）。</summary>
    public Transform PlayerTransform => playerTransform;

    /// <summary>巡邏左邊界。</summary>
    public Transform PatrolLeft => patrolPointLeft;

    /// <summary>巡邏右邊界。</summary>
    public Transform PatrolRight => patrolPointRight;

    /// <summary>目前血量。</summary>
    public int CurrentHealth => currentHealth;

    #endregion

    #region Unity 生命週期

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = enemyData.maxHealth;

        // 找尋場景中的玩家
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

        // 建立狀態
        stateMachine = new StateMachine();
        idleState   = new EnemyIdleState(this, stateMachine);
        chaseState  = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
        hurtState   = new EnemyHurtState(this, stateMachine);

        stateMachine.Initialize(idleState);
    }

    protected virtual void Update()
    {
        stateMachine.Update();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    #endregion

    #region IDamageable 實作

    /// <summary>
    /// 敵人受到傷害。
    /// </summary>
    public virtual void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        currentHealth -= damage;

        // 施加擊退
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackDirection.normalized * enemyData.hurtKnockbackForce, ForceMode2D.Impulse);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        stateMachine.ChangeState(hurtState);
    }

    /// <summary>
    /// 敵人死亡。
    /// </summary>
    public virtual void Die()
    {
        // TODO：播放死亡動畫、掉落道具
        Debug.Log($"[EnemyBase] {gameObject.name} 死亡！");
        Destroy(gameObject);
    }

    #endregion

    #region 公開方法

    /// <summary>
    /// 偵測玩家是否在偵測範圍內。
    /// </summary>
    /// <returns>是否偵測到玩家</returns>
    public bool IsPlayerInDetectionRange()
    {
        if (playerTransform == null) return false;
        return Vector2.Distance(transform.position, playerTransform.position) <= enemyData.detectionRange;
    }

    /// <summary>
    /// 偵測玩家是否在攻擊範圍內。
    /// </summary>
    /// <returns>是否在攻擊範圍內</returns>
    public bool IsPlayerInAttackRange()
    {
        if (playerTransform == null) return false;
        return Vector2.Distance(transform.position, playerTransform.position) <= enemyData.attackRange;
    }

    /// <summary>
    /// 翻轉敵人面向。
    /// </summary>
    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        if (enemyData == null) return;
        // 偵測範圍（黃色）
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyData.detectionRange);
        // 攻擊範圍（紅色）
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
    }

    #endregion
}
