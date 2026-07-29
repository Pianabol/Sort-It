using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private EGameState gameState;

    public static bool startInGameMode = true;

    // Şişeleri artık biz Start'ta aramıyoruz, LevelManager yaratınca buraya atıyor.
    public BottleController[] allBottles;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Start()
    {
        // FindObjectsByType KODU SİLİNDİ (Çakışmayı önlemek için)

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