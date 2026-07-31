using UnityEngine;
using TMPro; 

public class UIManager : MonoBehaviour, IGameStateListener
{
    [Header(" Panels ")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header(" UI Elements ")]
    [Tooltip("Arka planı (Image) olan Ana Popup Objesi")]
    [SerializeField] private GameObject levelPopupContainer; // YENİ EKLENDİ!
    
    [Tooltip("Sadece yazıyı değiştirmek için Text referansı")]
    [SerializeField] private TextMeshProUGUI levelPopupText;

    public void GameStateChanged(EGameState newState)
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (gamePanel) gamePanel.SetActive(false);
        if (levelCompletePanel) levelCompletePanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);

        switch (newState)
        {
            case EGameState.MENU:
                if (mainMenuPanel) mainMenuPanel.SetActive(true);
                break;
            case EGameState.GAME:
                if (gamePanel) gamePanel.SetActive(true);
                ShowLevelPopup();
                break;
            case EGameState.LEVELCOMPLETE:
                if (levelCompletePanel) levelCompletePanel.SetActive(true);
                break;
            case EGameState.GAMEOVER:
                if (gameOverPanel) gameOverPanel.SetActive(true);
                break;
        }
    }

    private void ShowLevelPopup()
    {
        // Eğer referansları Inspector'dan koymayı unuttuysan kod çökmesin diye güvenlik!
        if (levelPopupContainer == null || levelPopupText == null) return;

        // 1. Önce içindeki yazıyı dinamik olarak güncelle (Örn: "LEVEL 3")
        if (LevelManager.Instance != null)
        {
            levelPopupText.text = "LEVEL " + LevelManager.Instance.CurrentLevelNum.ToString();
        }

        // 2. Eğer oyuncu art arda hızlıca retry yaparsa eski animasyonlar çakışmasın diye iptal et
        LeanTween.cancel(levelPopupContainer);

        // 3. Obje görünür yap ve boyutunu sıfırla (Görünmezden başlayacak)
        levelPopupContainer.SetActive(true);
        levelPopupContainer.transform.localScale = Vector3.zero;

        // 4. Jöle gibi ekranda belir (0.4 saniye sürer)
        LeanTween.scale(levelPopupContainer, Vector3.one, 0.4f)
            .setEase(LeanTweenType.easeOutBack) 
            .setOnComplete(() =>
            {
                // 5. TAM 2 SANİYE EKRANDA KAL, sonra jöle gibi küçülüp kaybol (0.3 saniye sürer)
                LeanTween.scale(levelPopupContainer, Vector3.zero, 0.3f)
                    .setDelay(1.0f)  
                    .setEase(LeanTweenType.easeInBack)
                    .setOnComplete(() => 
                    {
                        levelPopupContainer.SetActive(false);
                    });
            });
    }
}