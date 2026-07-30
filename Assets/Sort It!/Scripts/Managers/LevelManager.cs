using System;
using UnityEngine;

public class LevelManager : MonoBehaviour, IGameStateListener
{
    public static LevelManager Instance;
    public static event Action<Level> OnLevelSpawned; // LEVEL DOĞDUĞUNDA FIRLATILACAK HABER

    [Header(" Levels ")]
    public GameObject[] levelPrefabs; 
    
    private GameObject currentLevelInstance;
    private const string LEVEL_KEY = "CurrentLevel";

    public int CurrentLevelNum => PlayerPrefs.GetInt(LEVEL_KEY, 0) + 1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    // Sahne yeniden yüklendiğinde (Retry/Next) kendini hafızadan temizler!
    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void SpawnLevel()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (levelPrefabs == null || levelPrefabs.Length == 0) return;

        int levelIndex = PlayerPrefs.GetInt(LEVEL_KEY, 0);
        int validatedIndex = levelIndex % levelPrefabs.Length;

        // 1. Level'ı sahnede doğur
        currentLevelInstance = Instantiate(levelPrefabs[validatedIndex], transform);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.allBottles = currentLevelInstance.GetComponentsInChildren<BottleController>();
        }

        // 2. Level doğduktan SONRA TimerManager'a haber ver ve doğan Level'ı gönder!
        Level spawnedLevel = currentLevelInstance.GetComponent<Level>();
        OnLevelSpawned?.Invoke(spawnedLevel);
    }

    public void GameStateChanged(EGameState newState)
    {
        if (newState == EGameState.GAME)
        {
            SpawnLevel();
        }
        else if (newState == EGameState.LEVELCOMPLETE)
        {
            int nextLevelIndex = PlayerPrefs.GetInt(LEVEL_KEY, 0) + 1;
            PlayerPrefs.SetInt(LEVEL_KEY, nextLevelIndex);
            PlayerPrefs.Save();
        }
    }
}