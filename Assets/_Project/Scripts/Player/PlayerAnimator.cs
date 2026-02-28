using UnityEngine;

/// <summary>
/// 玩家動畫控制橋接器：統一管理 Animator 組件的播放、布林值、浮點值等。
/// 從狀態機各狀態中呼叫此類別的方法，避免各狀態直接耦合 Animator。
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    #region 欄位

    private Animator animator;

    // 動畫參數名稱常數（避免拼寫錯誤）
    private static readonly int IsGrounded   = Animator.StringToHash("IsGrounded");
    private static readonly int IsRunning    = Animator.StringToHash("IsRunning");
    private static readonly int IsWallSlide  = Animator.StringToHash("IsWallSlide");
    private static readonly int VelocityY    = Animator.StringToHash("VelocityY");
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");
    private static readonly int HurtTrigger  = Animator.StringToHash("Hurt");
    private static readonly int DashTrigger  = Animator.StringToHash("Dash");

    #endregion

    #region Unity 生命週期

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    #endregion

    #region 公開方法

    /// <summary>
    /// 播放指定名稱的動畫狀態。
    /// </summary>
    /// <param name="animationName">動畫狀態名稱</param>
    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }

    /// <summary>
    /// 設定 Animator 布林參數。
    /// </summary>
    /// <param name="paramHash">參數 Hash（使用 Animator.StringToHash）</param>
    /// <param name="value">布林值</param>
    public void SetBool(int paramHash, bool value)
    {
        animator.SetBool(paramHash, value);
    }

    /// <summary>
    /// 設定 Animator 浮點參數。
    /// </summary>
    /// <param name="paramHash">參數 Hash</param>
    /// <param name="value">浮點值</param>
    public void SetFloat(int paramHash, float value)
    {
        animator.SetFloat(paramHash, value);
    }

    /// <summary>
    /// 觸發 Animator Trigger 參數。
    /// </summary>
    /// <param name="paramHash">參數 Hash</param>
    public void SetTrigger(int paramHash)
    {
        animator.SetTrigger(paramHash);
    }

    /// <summary>更新是否在地面的動畫參數。</summary>
    public void UpdateGrounded(bool isGrounded) => SetBool(IsGrounded, isGrounded);

    /// <summary>更新是否在奔跑的動畫參數。</summary>
    public void UpdateRunning(bool isRunning) => SetBool(IsRunning, isRunning);

    /// <summary>更新是否在滑牆的動畫參數。</summary>
    public void UpdateWallSlide(bool isWallSlide) => SetBool(IsWallSlide, isWallSlide);

    /// <summary>更新垂直速度動畫參數（用於跳躍/下落動畫）。</summary>
    public void UpdateVelocityY(float vy) => SetFloat(VelocityY, vy);

    /// <summary>播放攻擊動畫。</summary>
    public void TriggerAttack() => SetTrigger(AttackTrigger);

    /// <summary>播放受傷動畫。</summary>
    public void TriggerHurt() => SetTrigger(HurtTrigger);

    /// <summary>播放衝刺動畫。</summary>
    public void TriggerDash() => SetTrigger(DashTrigger);

    #endregion
}