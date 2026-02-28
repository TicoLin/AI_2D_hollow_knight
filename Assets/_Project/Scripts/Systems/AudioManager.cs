using UnityEngine;

/// <summary>
/// 音效管理器：管理背景音樂與音效的播放，繼承泛型單例。
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager>
{
    #region 欄位

    [Header("音量設定")]
    [Tooltip("音效音量（0 ~ 1）")]
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;

    [Tooltip("背景音樂音量（0 ~ 1）")]
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.7f;

    // 用於播放背景音樂的 AudioSource
    private AudioSource musicSource;

    // 用於播放音效的 AudioSource（重疊播放）
    private AudioSource sfxSource;

    #endregion

    #region Unity 生命週期

    protected override void Awake()
    {
        base.Awake();

        // 取得或建立兩個 AudioSource
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length >= 2)
        {
            musicSource = sources[0];
            sfxSource   = sources[1];
        }
        else
        {
            musicSource = GetComponent<AudioSource>();
            sfxSource   = gameObject.AddComponent<AudioSource>();
        }

        musicSource.loop = true;
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    #endregion

    #region 公開方法

    /// <summary>
    /// 播放音效（One Shot，可重疊）。
    /// </summary>
    /// <param name="clip">要播放的音效 AudioClip</param>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    /// <summary>
    /// 播放背景音樂（若與目前相同則不重新播放）。
    /// </summary>
    /// <param name="clip">要播放的音樂 AudioClip</param>
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    /// <summary>
    /// 停止背景音樂。
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }

    /// <summary>
    /// 設定音效音量。
    /// </summary>
    /// <param name="volume">音量值（0 ~ 1）</param>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
    }

    /// <summary>
    /// 設定背景音樂音量。
    /// </summary>
    /// <param name="volume">音量值（0 ~ 1）</param>
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    #endregion
}
