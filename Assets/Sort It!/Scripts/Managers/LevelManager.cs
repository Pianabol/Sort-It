using UnityEngine;

public class LevelManager : MonoBehaviour, IGameStateListener
{
    public static LevelManager Instance;

    [Header(" Levels ")]
    [Tooltip("Oluşturduğun Level Prefab'larını buraya sürükle")]
    public GameObject[] levelPrefabs; 
    
    private GameObject currentLevelInstance;
    private const string LEVEL_KEY = "CurrentLevel";

    public int CurrentLevelNum => PlayerPrefs.GetInt(LEVEL_KEY, 0) + 1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        // Oyuna girdiğimiz an doğrudan Level'ı yaratıp GameManager'a "BAŞLA" emri veriyoruz!
        LoadAndStartLevel();
    }

    public void LoadAndStartLevel()
    {
        // 1. ESKİ LEVELİ TEMİZLE
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (levelPrefabs == null || levelPrefabs.Length == 0)
        {
            Debug.LogError("Moruk LevelManager'da hiç prefab yok!");
            return;
        }

        // 2. LEVEL İNDEKSİNİ HESAPLA
        int levelIndex = PlayerPrefs.GetInt(LEVEL_KEY, 0);
        int validatedIndex = levelIndex % levelPrefabs.Length;

        // 3. LEVELİ YARAT
        currentLevelInstance = Instantiate(levelPrefabs[validatedIndex], transform);

        // 4. INSTANCE İLE DOĞRUDAN İLETİŞİM (Amelelik Bitti!):
        if (GameManager.Instance != null)
        {
            // A) Şişeleri GameManager'a ver
            GameManager.Instance.allBottles = currentLevelInstance.GetComponentsInChildren<BottleController>();
            
            // B) GameManager'a "Level hazır, oyunu başlat!" emri ver
            GameManager.Instance.StartGame();
        }
    }

    public void GameStateChanged(EGameState newState)
    {
        if (newState == EGameState.LEVELCOMPLETE)
        {
            // Level bittiğinde hafızadaki level numarasını artır
            int nextLevelIndex = PlayerPrefs.GetInt(LEVEL_KEY, 0) + 1;
            PlayerPrefs.SetInt(LEVEL_KEY, nextLevelIndex);
            PlayerPrefs.Save();
        }
    }
}