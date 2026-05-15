using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource audioSource;
    private bool isPlaying = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// 노래 재생 시작.
    /// </summary>
    public void PlaySong(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip이 null입니다.");
            return;
        }

        audioSource.clip = clip;
        audioSource.time = 0f;
        audioSource.Play();
        isPlaying = true;
    }

    public void StopSong()
    {
        audioSource.Stop();
        isPlaying = false;
    }

    public void PauseSong() { audioSource.Pause(); }
    public void ResumeSong() { audioSource.UnPause(); }

    /// <summary>
    /// 현재 곡 시간(초). RhythmManager가 이걸 기준으로 노트 타이밍을 계산.
    /// </summary>
    public float GetSongTime()
    {
        return isPlaying ? audioSource.time : 0f;
    }

    public bool IsPlaying() { return isPlaying && audioSource.isPlaying; }
}
