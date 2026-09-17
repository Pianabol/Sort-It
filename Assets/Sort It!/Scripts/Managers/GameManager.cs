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

        // 🚑 SİHİRLİ DOKUNUŞ: LeanTween Motorunu Zorla Sıfırla!
        // Oyunu durdurup başlattığında (PlayerPrefs temizlediğinde) animasyonların çökmesini %100 engeller.
        LeanTween.reset(); 
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

    public bool IsGame() => gameState == EGameState.GAME;

    // StartGame artık doğrudan geçiş metodunu kullanıyor, sıfır kod tekrarı!
    public void StartGame() => TransitionToState(EGameState.GAME);

    public void CheckGameWin()
    {
        if (!IsGame() || allBottles == null || allBottles.Length == 0) return;

        bool isGameWon = true;

        foreach (BottleController bottle in allBottles)
        {
            if (bottle == null) continue;

            bool isBottleComplete = bottle.numberOfColorsInBottle == 0 || 
                                   (bottle.numberOfColorsInBottle == 4 && bottle.numberOfTopColorLayers == 4);

            if (!isBottleComplete)
            {
                isGameWon = false;
                break;
            }
        }

        if (isGameWon)
        {
            Debug.Log("KAZANDIN MORUK! UI EKRANI GELİYOR!");
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
            
            if (source == null || source.numberOfColorsInBottle == 0 || 
               (source.numberOfColorsInBottle == 4 && source.numberOfTopColorLayers == 4)) continue;

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

    // === GECİKMESİZ (INSTANT) STATE MACHINE GEÇİŞLERİ ===

    public void HomeButtonCallBack() => TransitionToState(EGameState.MENU);
    public void NextButtonCallBack() => TransitionToState(EGameState.GAME);
    public void RetryButtonCallBack() => TransitionToState(EGameState.GAME);

    private void TransitionToState(EGameState targetState)
    {
        // 1. O an sahnede şişen, inen ne kadar UI veya animasyon varsa şak diye kes (Bug'ları önler)
        LeanTween.cancelAll(); 

        // 2. Işık hızında yeni duruma geç (LevelManager yeni leveli anında doğurur)
        SetGameState(targetState);

        // 3. Eğer oyuna giriyorsak güvenlik kontrolünü yap
        if (targetState == EGameState.GAME)
        {
            CheckGameWin();
        }
    }
}