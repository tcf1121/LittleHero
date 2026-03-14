using UnityEngine;

// 사운드 관리
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // 효과음 재생
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // 배경음 재생
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        // 이미 같은 곡이 재생 중이면 무시
        if (bgmSource.clip == clip) return;

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }
}
