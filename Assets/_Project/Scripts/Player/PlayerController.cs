using UnityEngine;

/// <summary>
/// 玩家主控制器：管理輸入、物理偵測、狀態機初始化與更新。
/// 持有所有玩家子系統的引用，作為各狀態的資料中心。
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(PlayerHealth))]
public class PlayerController : MonoBehaviour
{
    #region 欄位

    [Header("資料")]
    [Tooltip("玩家數值配置 ScriptableObject")]
    [SerializeField] private PlayerData playerData;

    // 子系統引用
    private StateMachine stateMachine;
    private Rigidbody2D rb;
    private PlayerAnimator playerAnimator;
    private PlayerCombat playerCombat;
    private PlayerHealth playerHealth;

    // 輸入快取
    private float horizontalInput;
    private bool jumpInputDown;
    private bool jumpInputHeld;
    private bool attackInputDown;
    private bool dashInputDown;

    // 物理偵測快取
    private bool isGrounded;
    private bool isTouchingWallRight;
    private bool isTouchingWallLeft;

    // 計時器
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private float lastDashTime = -999f;

    // 額外跳躍次數
    private int remainingExtraJumps;

    // 面朝方向（1 = 右，-1 = 左）
    private int facingDirection = 1;

    // 所有狀態引用
    private PlayerIdleState idleState;
    private PlayerRunState runState;
    private PlayerJumpState jumpState;
    private PlayerFallState fallState;
    private PlayerWallSlideState wallSlideState;
    private PlayerWallJumpState wallJumpState;
    private PlayerDashState dashState;
    private PlayerAttackState attackState;
    private PlayerHurtState hurtState;

    #endregion

    #region 屬性（供狀態讀取）

    /// <summary>玩家數值配置。</summary>
    public PlayerData Data => playerData;

    /// <summary>Rigidbody2D 引用。</summary>
    public Rigidbody2D Rb => rb;

    /// <summary>動畫控制器引用。</summary>
    public PlayerAnimator Animator => playerAnimator;

    /// <summary>戰鬥系統引用。</summary>
    public PlayerCombat Combat => playerCombat;

    /// <summary>血量系統引用。</summary>
    public PlayerHealth Health => playerHealth;

    /// <summary>狀態機引用。</summary>
    public StateMachine StateMachine => stateMachine;

    /// <summary>水平輸入值（-1 ~ 1）。</summary>
    public float HorizontalInput => horizontalInput;

    /// <summary>本幀是否按下跳躍鍵。</summary>
    public bool JumpInputDown => jumpInputDown;

    /// <summary>跳躍鍵是否持續按住。</summary>
    public bool JumpInputHeld => jumpInputHeld;

    /// <summary>本幀是否按下攻擊鍵。</summary>
    public bool AttackInputDown => attackInputDown;

    /// <summary>本幀是否按下衝刺鍵。</summary>
    public bool DashInputDown => dashInputDown;

    /// <summary>是否踩在地面上。</summary>
    public bool IsGrounded => isGrounded;

    /// <summary>是否貼著右側牆壁。</summary>
    public bool IsTouchingWallRight => isTouchingWallRight;

    /// <summary>是否貼著左側牆壁。</summary>
    public bool IsTouchingWallLeft => isTouchingWallLeft;

    /// <summary>是否貼著任一側牆壁。</summary>
    public bool IsTouchingWall => isTouchingWallRight || isTouchingWallLeft;

    /// <summary>土狼時間計時器剩餘值。</summary>
    public float CoyoteTimeCounter => coyoteTimeCounter;

    /// <summary>跳躍緩衝計時器剩餘值。</summary>
    public float JumpBufferCounter => jumpBufferCounter;

    /// <summary>剩餘額外跳躍次數。</summary>
    public int RemainingExtraJumps => remainingExtraJumps;

    /// <summary>面朝方向（1 = 右，-1 = 左）。</summary>
    public int FacingDirection => facingDirection;

    /// <summary>衝刺是否可用（不在冷卻中）。</summary>
    public bool CanDash => Time.time >= lastDashTime + playerData.dashCooldown;

    // 狀態引用存取
    public PlayerIdleState IdleState => idleState;
    public PlayerRunState RunState => runState;
    public PlayerJumpState JumpState => jumpState;
    public PlayerFallState FallState => fallState;
    public PlayerWallSlideState WallSlideState => wallSlideState;
    public PlayerWallJumpState WallJumpState => wallJumpState;
    public PlayerDashState DashState => dashState;
    public PlayerAttackState AttackState => attackState;
    public PlayerHurtState HurtState => hurtState;

    #endregion

    #region Unity 生命週期

    private void Awake()
    {
        // 取得組件引用
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<PlayerAnimator>();
        playerCombat = GetComponent<PlayerCombat>();
        playerHealth = GetComponent<PlayerHealth>();

        // 建立所有狀態
        stateMachine = new StateMachine();
        idleState     = new PlayerIdleState(this, stateMachine, playerData);
        runState      = new PlayerRunState(this, stateMachine, playerData);
        jumpState     = new PlayerJumpState(this, stateMachine, playerData);
        fallState     = new PlayerFallState(this, stateMachine, playerData);
        wallSlideState = new PlayerWallSlideState(this, stateMachine, playerData);
        wallJumpState  = new PlayerWallJumpState(this, stateMachine, playerData);
        dashState      = new PlayerDashState(this, stateMachine, playerData);
        attackState    = new PlayerAttackState(this, stateMachine, playerData);
        hurtState      = new PlayerHurtState(this, stateMachine, playerData);

        // 初始化狀態機
        stateMachine.Initialize(idleState);

        // 初始化跳躍次數
        remainingExtraJumps = playerData.extraJumpCount;
    }

    private void Update()
    {
        GatherInput();
        UpdateTimers();
        CheckPhysics();
        stateMachine.Update();

        // 重置單幀輸入
        jumpInputDown = false;
        attackInputDown = false;
        dashInputDown = false;
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    #endregion

    #region 私有方法

    /// <summary>收集本幀的玩家輸入。</summary>
    private void GatherInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            jumpInputDown = true;
            jumpBufferCounter = playerData.jumpBufferTime;
        }

        jumpInputHeld = Input.GetButton("Jump");

        // J 鍵攻擊
        if (Input.GetKeyDown(KeyCode.J))
            attackInputDown = true;

        // K 鍵衝刺
        if (Input.GetKeyDown(KeyCode.K))
            dashInputDown = true;
    }

    /// <summary>更新各計時器。</summary>
    private void UpdateTimers()
    {
        // 土狼時間：在地面時重置，離地後開始倒數
        if (isGrounded)
            coyoteTimeCounter = playerData.coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // 跳躍緩衝倒數
        jumpBufferCounter -= Time.deltaTime;
    }

    /// <summary>進行地面與牆壁的 BoxCast 物理偵測。</summary>
    private void CheckPhysics()
    {
        // 地面偵測
        isGrounded = Physics2D.BoxCast(
            transform.position,
            playerData.groundCheckSize,
            0f,
            Vector2.down,
            playerData.groundCheckDistance,
            playerData.groundLayer
        );

        // 右側牆壁偵測
        isTouchingWallRight = Physics2D.BoxCast(
            transform.position,
            playerData.wallCheckSize,
            0f,
            Vector2.right,
            playerData.wallCheckDistance,
            playerData.wallLayer
        );

        // 左側牆壁偵測
        isTouchingWallLeft = Physics2D.BoxCast(
            transform.position,
            playerData.wallCheckSize,
            0f,
            Vector2.left,
            playerData.wallCheckDistance,
            playerData.wallLayer
        );
    }

    #endregion

    #region 公開方法

    /// <summary>
    /// 重置剩餘額外跳躍次數（通常在著地時呼叫）。
    /// </summary>
    public void ResetExtraJumps()
    {
        remainingExtraJumps = playerData.extraJumpCount;
    }

    /// <summary>
    /// 消耗一次額外跳躍次數。
    /// </summary>
    public void UseExtraJump()
    {
        remainingExtraJumps--;
    }

    /// <summary>
    /// 記錄衝刺使用時間（重置冷卻）。
    /// </summary>
    public void UseDash()
    {
        lastDashTime = Time.time;
    }

    /// <summary>
    /// 翻轉角色面向方向。
    /// </summary>
    /// <param name="direction">目標方向（正數 = 右，負數 = 左）</param>
    public void Flip(float direction)
    {
        if (direction > 0 && facingDirection == -1)
        {
            facingDirection = 1;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (direction < 0 && facingDirection == 1)
        {
            facingDirection = -1;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    /// <summary>
    /// 由 PlayerHealth 呼叫，通知控制器進入受傷狀態。
    /// </summary>
    /// <param name="knockbackDirection">擊退方向</param>
    public void OnHurt(Vector2 knockbackDirection)
    {
        stateMachine.ChangeState(hurtState);
    }

    /// <summary>
    /// 消耗跳躍緩衝（已執行跳躍後重置）。
    /// </summary>
    public void UseJumpBuffer()
    {
        jumpBufferCounter = 0f;
    }

    #endregion
}
