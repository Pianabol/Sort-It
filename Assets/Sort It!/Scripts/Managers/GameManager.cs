using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private EGameState gameState;

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
    
    private void Start()
    {
        SetGameState(EGameState.MENU);
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
        SetGameState(EGameState.GAME);
        CheckGameWin(); // Hacker Koruması (Baştan kilitli level kontrolü)
    }

    public void CheckGameWin()
    {
        if (!IsGame()) return; 
        if (allBottles == null || allBottles.Length == 0) return;

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
            // YILDIZ HESAPLAMA SİLİNDİ! Direkt Level Complete'e geçiyor.
            SetGameState(EGameState.LEVELCOMPLETE); 
            return; 
        }

        if (!HasAvailableMoves())
        {
            Debug.Log("HAMLE KALMADI KUZEN! GAME OVER EKRANI GELİYOR!");
            SetGameState(EGameState.GAMEOVER);
        }
    }

    private bool HasAvailableMoves()
    {
        for (int i = 0; i < allBottles.Length; i++)
        {
            BottleController source = allBottles[i];
            
            if (source == null || source.numberOfColorsInBottle == 0) continue;
            if (source.numberOfColorsInBottle == 4 && source.numberOfTopColorLayers == 4) continue;

            source.UpdateTopColorValues();

            for (int j = 0; j < allBottles.Length; j++)
            {
                if (i == j) continue;

                BottleController target = allBottles[j];
                
                if (target == null || target.numberOfColorsInBottle == 4) continue;

                target.UpdateTopColorValues();

                if (target.FillBottleCheck(source.topColor))
                {
                    return true; 
                }
            }
        }
        return false;
    }

    public void HomeButtonCallBack()
    {
        if (CanvasFader.Instance != null) CanvasFader.Instance.FadeOut(() => { SceneManager.LoadScene(0); });
        else SceneManager.LoadScene(0);
    }

    public void NextButtonCallBack()
    {
        if (CanvasFader.Instance != null) CanvasFader.Instance.FadeOut(() => { SceneManager.LoadScene(0); });
        else SceneManager.LoadScene(0);
    }

    public void RetryButtonCallBack()
    {
        if (CanvasFader.Instance != null) CanvasFader.Instance.FadeOut(() => { SceneManager.LoadScene(0); });
        else SceneManager.LoadScene(0);
    }
}