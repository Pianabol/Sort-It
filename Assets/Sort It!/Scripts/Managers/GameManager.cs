using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private EGameState gameState;

    public static bool startInGameMode = true;

    public BottleController[] allBottles;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // YENİ EKLENDİ: Sahne yenilendiğinde eski kalıntıları temizle (Hayalet referansları engeller)
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Start()
    {
        allBottles = FindObjectsByType<BottleController>(FindObjectsSortMode.None);

        if (startInGameMode)
        {
            startInGameMode = false; 
            StartGame();
        }
        else
        {
            SetGameState(EGameState.MENU);
        }
    }

    public void SetGameState(EGameState newState)
    {
        this.gameState = newState;

        IEnumerable<IGameStateListener> gameStateListeners
            = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IGameStateListener>();

        foreach (IGameStateListener dependency in gameStateListeners)
        {
            dependency.GameStateChanged(newState);
        }
    }

    public bool IsGame()
    {
        return gameState == EGameState.GAME;
    }

    public void StartGame()
    {
        if (ScoreManager.Instance != null) ScoreManager.Instance.ResetScore();
        SetGameState(EGameState.GAME);
    }

    public void CheckGameWin()
    {
        if (!IsGame()) return; 

        bool isGameWon = true;

        foreach (BottleController bottle in allBottles)
        {
            // === YENİ EKLENDİ: GÜVENLİK KİLİDİ ===
            // Eğer sahne yenilenirken vs. şişe silinmişse, hata vermeden diğerine geç!
            if (bottle == null) continue;

            bool isBottleComplete = false;

            if (bottle.numberOfColorsInBottle == 0)
            {
                isBottleComplete = true;
            }
            else if (bottle.numberOfColorsInBottle == 4 && bottle.numberOfTopColorLayers == 4)
            {
                isBottleComplete = true;
            }

            if (isBottleComplete == false)
            {
                isGameWon = false;
                break;
            }
        }

        if (isGameWon)
        {
            Debug.Log("KAZANDIN MORUK! UI EKRANI GELİYOR!");
            if (ScoreManager.Instance != null) ScoreManager.Instance.CalculateStars();
            SetGameState(EGameState.LEVELCOMPLETE); 
        }
    }

    public void HomeButtonCallBack()
    {
        if (CanvasFader.Instance != null)
        {
            CanvasFader.Instance.FadeOut(() => {
                startInGameMode = false; 
                SceneManager.LoadScene(0);
            });
        }
        else
        {
            startInGameMode = false;
            SceneManager.LoadScene(0);
        }
    }

    public void NextButtonCallBack()
    {
        int nextLevelIndex = PlayerPrefs.GetInt("CurrentLevel", 0) + 1;
        PlayerPrefs.SetInt("CurrentLevel", nextLevelIndex);
        PlayerPrefs.Save();

        if (CanvasFader.Instance != null)
        {
            CanvasFader.Instance.FadeOut(() => {
                startInGameMode = true;  
                SceneManager.LoadScene(0);
            });
        }
        else
        {
            startInGameMode = true;
            SceneManager.LoadScene(0);
        }
    }

    public void RetryButtonCallBack()
    {
        if (CanvasFader.Instance != null)
        {
            CanvasFader.Instance.FadeOut(() => {
                startInGameMode = true;  
                SceneManager.LoadScene(0);
            });
        }
        else
        {
            startInGameMode = true;
            SceneManager.LoadScene(0);
        }
    }
}