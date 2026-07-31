using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header(" Audio Source ")]
    [Tooltip("Tüm ses efektlerini çalacak olan ana kaynak")]
    [SerializeField] private AudioSource sfxSource;

    [Header(" Audio Clips (Ses Dosyaları) ")]
    public AudioClip clickSound;
    public AudioClip bottleSelectSound;
    public AudioClip pourSound;
    public AudioClip winSound;
    public AudioClip loseSound;

    private void Awake()
    {
        // Singleton Deseni
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Kod içinden çağrılacak ana metod (AAA Standartı: PlayOneShot)
    // PlayOneShot, seslerin üst üste binip kesilmesini engeller!
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // === UI BUTONLARI İÇİN HAZIR METODLAR ===
    // Inspector'daki OnClick() eventlerine direkt bunları bağlayabilirsin!
    public void PlayClickSound() => PlaySFX(clickSound);
    public void PlayWinSound() => PlaySFX(winSound);
    public void PlayLoseSound() => PlaySFX(loseSound);
}