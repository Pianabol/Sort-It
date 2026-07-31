/*
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
        if (CanvasFader.Instance != null)
        {
            CanvasFader.Instance.FadeOut(() => 
            {
                try
                {
                    SetGameState(EGameState.GAME);
                    CheckGameWin();
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Oyun başlatılırken arkaplanda hata oluştu moruk: " + e.ToString());
                }
                finally
                {
                    // Ne olursa olsun (hata çıksa bile) ekran siyah kalmasın, eriyip açılsın!
                    CanvasFader.Instance.FadeIn(); 
                }
            });
        }
        else
        {
            SetGameState(EGameState.GAME);
            CheckGameWin();
        }
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
} */


using System.Collections; // YENİ: Coroutine'ler için gerekli!
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
        if (CanvasFader.Instance != null)
        {
            CanvasFader.Instance.FadeOut(() => 
            {
                try
                {
                    SetGameState(EGameState.GAME);
                    CheckGameWin();
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Oyun başlatılırken arkaplanda hata oluştu moruk: " + e.ToString());
                }
                finally
                {
                    // Ne olursa olsun (hata çıksa bile) ekran siyah kalmasın, eriyip açılsın!
                    CanvasFader.Instance.FadeIn(); 
                }
            });
        }
        else
        {
            SetGameState(EGameState.GAME);
            CheckGameWin();
        }
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

    /*
    // === BUTONLAR İÇİN AAA KALİTESİNDE GÜVENLİ SAHNE YÜKLEME SİSTEMİ ===

    public void HomeButtonCallBack() => StartCoroutine(ReloadSceneSafe());
    public void NextButtonCallBack() => StartCoroutine(ReloadSceneSafe());
    public void RetryButtonCallBack() => StartCoroutine(ReloadSceneSafe());

    private IEnumerator ReloadSceneSafe()
    {
        if (CanvasFader.Instance != null)
        {
            bool isFaded = false;
            
            // 1. Ekranı karart ve bittiğinde bayrağı (isFaded) true yap
            CanvasFader.Instance.FadeOut(() => { isFaded = true; });
            
            // 2. Kararma bitene kadar kodu burada dondur!
            yield return new WaitUntil(() => isFaded);
        }

        // 3. ZIRH: Sahne değişirken LeanTween motorunun çökmemesi için tüm animasyonları temizle!
        LeanTween.cancelAll();

        // 4. ZIRH: Yanlışlıkla 0. indexteki SampleScene'e gitmek yerine, ŞU AN OYNADIĞIN sahneyi bul ve yeniden yükle!
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    */

    public void HomeButtonCallBack() => StartCoroutine(TransitionToState(EGameState.MENU));
    
    // LevelComplete anında PlayerPrefs 1 arttığı için GAME demek yeni leveli açar!
    public void NextButtonCallBack() => StartCoroutine(TransitionToState(EGameState.GAME));
    
    // GameOver'da PlayerPrefs artmadığı için GAME demek aynı leveli baştan açar!
    public void RetryButtonCallBack() => StartCoroutine(TransitionToState(EGameState.GAME));

    private IEnumerator TransitionToState(EGameState targetState)
    {
        // 1. Ekranı Karart ve bekle
        if (CanvasFader.Instance != null)
        {
            bool isFaded = false;
            CanvasFader.Instance.FadeOut(() => { isFaded = true; });
            yield return new WaitUntil(() => isFaded);
        }

        // 2. Şişme sönme vb. animasyonları temizle ki çakışmasın
        LeanTween.cancelAll(); 

        // 3. SİHİR BURADA: Sahne yüklemek yerine diğer Manager'lara emir veriyoruz!
        SetGameState(targetState);

        // 4. Eğer oyuna (Next veya Retry) giriliyorsa hacker/kilit kontrolünü yap
        if (targetState == EGameState.GAME)
        {
            CheckGameWin();
        }

        // 5. Her şey dizilip hazır olduğuna göre ekranı aydınlat
        if (CanvasFader.Instance != null)
        {
            CanvasFader.Instance.FadeIn();
        }
    }
}