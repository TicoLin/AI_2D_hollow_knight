using UnityEngine;

/// <summary>
/// 迷你 Boss 控制器：繼承 BossBase，初始化迷你 Boss 特有的狀態機。
/// </summary>
public class MiniBossController : BossBase
{
    #region 欄位

    [Header("投射物")]
    [Tooltip("投射物 Prefab（Phase 2 使用）")]
    [SerializeField] private GameObject projectilePrefab;

    [Tooltip("投射物發射點")]
    [SerializeField] private Transform projectileSpawnPoint;

    // 狀態引用
    private MiniBossIdleState miniBossIdleState;
    private MiniBossDashAttackState dashAttackState;
    private MiniBossSlamState slamState;
    private MiniBossComboState comboState;
    private MiniBossProjectileState projectileState;
    private MiniBossStaggerState staggerState;

    #endregion

    #region 屬性

    public GameObject ProjectilePrefab => projectilePrefab;
    public Transform ProjectileSpawnPoint => projectileSpawnPoint;

    public MiniBossIdleState IdleState => miniBossIdleState;
    public MiniBossDashAttackState DashAttackState => dashAttackState;
    public MiniBossSlamState SlamState => slamState;
    public MiniBossComboState ComboState => comboState;
    public MiniBossProjectileState ProjectileState => projectileState;
    public MiniBossStaggerState StaggerState => staggerState;

    #endregion

    #region Unity 生命週期

    protected override void Awake()
    {
        base.Awake();

        // 建立迷你 Boss 特有狀態
        miniBossIdleState = new MiniBossIdleState(this, stateMachine);
        dashAttackState   = new MiniBossDashAttackState(this, stateMachine);
        slamState         = new MiniBossSlamState(this, stateMachine);
        comboState        = new MiniBossComboState(this, stateMachine);
        projectileState   = new MiniBossProjectileState(this, stateMachine);
        staggerState      = new MiniBossStaggerState(this, stateMachine);

        // 初始化狀態機
        stateMachine.Initialize(miniBossIdleState);
    }

    #endregion

    #region 覆寫方法

    /// <summary>
    /// 覆寫受傷方法，加入硬直累積判斷。
    /// </summary>
    public override void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        base.TakeDamage(damage, knockbackDirection);

        // 若累積傷害達到硬直閾值，進入硬直狀態
        if (damageAccumulator >= bossData.staggerThreshold)
        {
            damageAccumulator = 0;
            stateMachine.ChangeState(staggerState);
        }
    }

    /// <summary>
    /// Phase 切換時的行為（Phase 2 開始可使用投射物）。
    /// </summary>
    protected override void OnPhaseChanged(int newPhase)
    {
        Debug.Log($"[MiniBossController] 進入 Phase {newPhase}，攻擊變得更激進！");
    }

    #endregion
}
