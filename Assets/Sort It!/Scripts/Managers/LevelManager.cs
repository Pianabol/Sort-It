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

    // DİKKAT: Start() metodunu sildik ki GameManager ile aynı anda 2 kez level üretmesin!

    private void SpawnLevel()
    {
        // 1. ESKİ LEVELİ TEMİZLE
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (levelPrefabs == null || levelPrefabs.Length == 0) return;

        // 2. LEVEL İNDEKSİNİ HESAPLA (Modulo ile sonsuz döngü)
        int levelIndex = PlayerPrefs.GetInt(LEVEL_KEY, 0);
        int validatedIndex = levelIndex % levelPrefabs.Length;

        // 3. LEVELİ YARAT
        currentLevelInstance = Instantiate(levelPrefabs[validatedIndex], transform);

        // 4. KUSURSUZ İLETİŞİM: GameManager'a "Git sahnede ara" demek yerine,
        // direkt kendi yarattığımız levelin içindeki şişeleri veriyoruz! (10 kat daha performanslı)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.allBottles = currentLevelInstance.GetComponentsInChildren<BottleController>();
        }
    }

    public void GameStateChanged(EGameState newState)
    {
        if (newState == EGameState.GAME)
        {
            // State Game olduğunda Level'i yarat!
            SpawnLevel();
        }
        else if (newState == EGameState.LEVELCOMPLETE)
        {
            // Level bittiği an hafızadaki level değerini +1 artırıyoruz
            int nextLevelIndex = PlayerPrefs.GetInt(LEVEL_KEY, 0) + 1;
            PlayerPrefs.SetInt(LEVEL_KEY, nextLevelIndex);
            PlayerPrefs.Save();
        }
    }
}