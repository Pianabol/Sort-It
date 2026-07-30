using TMPro;
using UnityEngine;
using System;

public class TimerManager : MonoBehaviour, IGameStateListener
{
    public static TimerManager Instance;

    [Header(" Timer Settings ")]
    [Tooltip("Game Panel içindeki süreyi gösterecek Text objesi")]
    [SerializeField] private TextMeshProUGUI timerText;
    
    private int remainingTime;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void StartTimer()
    {
        StopTimer(); // Garanti olsun diye eski sayacı temizle (Çift geri sayımı engeller)
        InvokeRepeating(nameof(UpdateTimer), 1f, 1f); // 1 saniye bekle, sonra saniyede 1 kere çalış!
    }

    private void UpdateTimer()
    {
        remainingTime--;
        UpdateTimerText();

        if (remainingTime <= 0)
        {
            TimerEnded();
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            // Süreyi 01:30 gibi şık bir mm:ss formatına çevirir
            timerText.text = TimeSpan.FromSeconds(remainingTime).ToString(@"mm\:ss");
        }
    }

    private void StopTimer()
    {
        CancelInvoke(nameof(UpdateTimer));
    }

    private void TimerEnded()
    {
        StopTimer();
        Debug.Log("SÜRE BİTTİ KUZEN! FİŞİ ÇEKİYORUZ, GAME OVER!");
        
        // GameManager ile kusursuz iletişim: Süre bittiyse oyunu kaybetme durumuna al!
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetGameState(EGameState.GAMEOVER);
        }
    }

    // === STATE MACHINE (Oyunun Durumunu Dinleyen Kulak) ===
    public void GameStateChanged(EGameState newState)
    {
        if (newState == EGameState.GAME)
        {
            // 1. Sahnede doğan (Spawn olan) o anki Level objesini bul
            Level currentLevel = FindFirstObjectByType<Level>();

            // 2. Süreyi o levelin Inspector'ından çek!
            if (currentLevel != null)
            {
                remainingTime = currentLevel.LevelDuration;
            }
            else
            {
                // Güvenlik Kilidi: Eğer bir level prefabına Level.cs koymayı unutursan oyun çökmesin!
                Debug.LogWarning("Moruk bu Level Prefab'ında Level.cs yok! Varsayılan olarak 90 saniye veriyorum.");
                remainingTime = 90; 
            }

            // 3. UI'ı güncelle ve Sayacı başlat
            UpdateTimerText();
            StartTimer();
        }
        else if (newState == EGameState.LEVELCOMPLETE || newState == EGameState.GAMEOVER || newState == EGameState.MENU)
        {
            // Oyun kazanıldığında, kaybedildiğinde veya menüye dönüldüğünde süreyi dondur!
            StopTimer();
        }
    }
}