using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header(" Audio Source ")]
    [SerializeField] private AudioSource sfxSource;

    private AudioSource pourAudioSource; // Dökülme sesini yöneten özel kaynak

    [Header(" Audio Clips ")]
    public AudioClip clickSound;
    public AudioClip bottleSelectSound;
    public AudioClip pourSound;
    public AudioClip winSound;
    public AudioClip loseSound;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Dökülme sesini anında başlatıp kesebilmek için dinamik hoparlör oluşturuyoruz
        pourAudioSource = gameObject.AddComponent<AudioSource>();
        pourAudioSource.playOnAwake = false;
        pourAudioSource.loop = true; // Sıvı aktığı sürece döner
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Sıvı çizgisi çıktığı an çalışır
    public void StartPourSound()
    {
        if (pourSound == null || pourAudioSource == null) return;
        
        pourAudioSource.clip = pourSound;
        pourAudioSource.volume = 1f;
        pourAudioSource.Play();
    }

    // Sıvı çizgisi kapandığı an sesi yumuşakça keser (Pıt sesi yapmaz)
    public void StopPourSound()
    {
        if (pourAudioSource == null || !pourAudioSource.isPlaying) return;
        StartCoroutine(FadeOutPourRoutine());
    }

    private IEnumerator FadeOutPourRoutine()
    {
        float startVol = pourAudioSource.volume;
        float t = 0f;
        
        while (t < 0.15f)
        {
            t += Time.deltaTime;
            pourAudioSource.volume = Mathf.Lerp(startVol, 0f, t / 0.15f);
            yield return null;
        }

        pourAudioSource.Stop();
        pourAudioSource.volume = startVol;
    }

    public void PlayClickSound() => PlaySFX(clickSound);
    public void PlayWinSound() => PlaySFX(winSound);
    public void PlayLoseSound() => PlaySFX(loseSound);
}