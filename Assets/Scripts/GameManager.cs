using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton Pattern: Bu class'a her yerden tek kelimeyle ulaşmamızı sağlar.
    public static GameManager Instance;

    public BottleController[] allBottles;

    void Awake()
    {
        // GameManager'ın tek bir tane olduğundan emin oluyoruz
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        // LEVEL BAŞLARKEN: Sahnedeki tüm şişeleri otomatik bulup listeye ekler. 
        // Böylece Inspector'dan tek tek şişe sürüklemene gerek kalmaz!
        allBottles = FindObjectsOfType<BottleController>();
    }

    // Bu metot, her sıvı dökme işlemi bittiğinde BottleController tarafından çağrılacak
    public void CheckGameWin()
    {
        bool isGameWon = true;

        foreach (BottleController bottle in allBottles)
        {
            bool isBottleComplete = false;

            // KURAL 1: Eğer şişe tamamen BOŞ ise (0 renk), bu şişe OKEY'dir.
            if (bottle.numberOfColorsInBottle == 0)
            {
                isBottleComplete = true;
            }
            // KURAL 2: Eğer şişe tamamen DOLU ise (4) VE içindeki tüm renkler aynıysa (4 katman), OKEY'dir.
            else if (bottle.numberOfColorsInBottle == 4 && bottle.numberOfTopColorLayers == 4)
            {
                isBottleComplete = true;
            }

            // Eğer tek bir şişe bile bu iki kurala uymuyorsa, oyun henüz bitmemiştir!
            if (isBottleComplete == false)
            {
                isGameWon = false;
                break; // Diğer şişelere bakmaya gerek yok, döngüyü anında kır (Performans tasarrufu).
            }
        }

        // Eğer döngü bittiğinde isGameWon hala true ise, tüm şişeler OKEY demektir!
        if (isGameWon)
        {
            Debug.Log("KAZANDIN MORUK! BÜTÜN ŞİŞELER TAMAM!");
            // Not: Listemizdeki UIManager maddesine geçtiğimizde buraya UIManager.ShowWinScreen() yazacağız.
        }
    }
}