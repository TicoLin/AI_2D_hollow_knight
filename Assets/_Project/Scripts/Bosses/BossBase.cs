using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Boss 基底類別：實作 IDamageable，管理 Boss 的血量、階段切換與狀態機。
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class BossBase : MonoBehaviour, IDamageable
{
    #region 欄位

    [Header("資料")]
    [Tooltip("Boss 數值配置")]
    [SerializeField] protected BossData bossData;

    [Header("事件頻道")]
    [Tooltip("Boss 血量變化事件（攜帶目前血量）")]
    [SerializeField] protected IntEventChannel onBossHealthChanged;

    [Tooltip("Boss 死亡事件")]
    [SerializeField] protected VoidEventChannel onBossDied;

    [Tooltip("Boss 階段切換事件（攜帶新階段編號）")]
    [SerializeField] protected IntEventChannel onPhaseChanged;

    // 組件
    protected StateMachine stateMachine;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Transform playerTransform;

    // 血量與階段
    protected int currentHealth;
    protected int currentPhase = 1;

    // 硬直累積傷害計數
    protected int damageAccumulator = 0;

    #endregion

    #region 屬性

    /// <summary>Boss 數值配置。</summary>
    public BossData Data => bossData;

    /// <summary>Rigidbody2D 引用。</summary>
    public Rigidbody2D Rb => rb;

    /// <summary>Animator 引用。</summary>
    public Animator Anim => animator;

    /// <summary>玩家 Transform。</summary>
    public Transform PlayerTransform => playerTransform;

    /// <summary>目前血量。</summary>
    public int CurrentHealth => currentHealth;

    /// <summary>目前 Boss 階段。</summary>
    public int CurrentPhase => currentPhase;

    /// <summary>Boss 階段切換事件。</summary>
    public UnityAction<int> OnPhaseChange;

    #endregion

    #region Unity 生命週期

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = bossData.maxHealth;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

        stateMachine = new StateMachine();
    }

    protected virtual void Update()
    {
        stateMachine.Update();
        CheckPhase();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    #endregion

    #region IDamageable 實作

    /// <summary>
    /// Boss 接受傷害。
    /// </summary>
    public virtual void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        damageAccumulator += damage;

        // 廣播血量變化
        onBossHealthChanged?.RaiseEvent(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }
    }

    /// <summary>
    /// Boss 死亡。
    /// </summary>
    public virtual void Die()
    {
        onBossDied?.RaiseEvent();
        Debug.Log($"[BossBase] {bossData.bossName} 已被擊敗！");
        // TODO：死亡動畫、獎勵等
        Destroy(gameObject);
    }

    #endregion

    #region 私有方法

    /// <summary>根據 HP 百分比檢查是否需要切換階段。</summary>
    private void CheckPhase()
    {
        if (bossData.phaseThresholds == null) return;

        float healthPercent = (float)currentHealth / bossData.maxHealth;

        for (int i = 0; i < bossData.phaseThresholds.Length; i++)
        {
            int phase = i + 2; // Phase 2, 3, ...
            if (healthPercent <= bossData.phaseThresholds[i] && currentPhase < phase)
            {
                currentPhase = phase;
                OnPhaseChange?.Invoke(currentPhase);
                onPhaseChanged?.RaiseEvent(currentPhase);
                Debug.Log($"[BossBase] {bossData.bossName} 進入 Phase {currentPhase}！");
                OnPhaseChanged(currentPhase);
            }
        }
    }

    #endregion

    #region 虛方法

    /// <summary>
    /// 當階段切換時呼叫（子類別覆寫以實作階段特定行為）。
    /// </summary>
    /// <param name="newPhase">新階段編號</param>
    protected virtual void OnPhaseChanged(int newPhase) { }

    #endregion
}
