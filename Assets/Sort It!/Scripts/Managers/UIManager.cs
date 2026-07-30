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
        if (levelPopupText == null) return;

        if (LevelManager.Instance != null)
        {
            levelPopupText.text = "LEVEL " + LevelManager.Instance.CurrentLevelNum.ToString();
        }

        levelPopupText.gameObject.SetActive(true);
        levelPopupText.transform.localScale = Vector3.zero;

        LeanTween.scale(levelPopupText.gameObject, Vector3.one, 0.4f)
            .setEase(LeanTweenType.easeOutBack) 
            .setOnComplete(() =>
            {
                LeanTween.scale(levelPopupText.gameObject, Vector3.zero, 0.3f)
                    .setDelay(1.2f) 
                    .setEase(LeanTweenType.easeInBack)
                    .setOnComplete(() => 
                    {
                        levelPopupText.gameObject.SetActive(false);
                    });
            });
    }
}