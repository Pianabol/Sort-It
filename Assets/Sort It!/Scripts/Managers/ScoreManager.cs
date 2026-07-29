using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public static event Action<int, int> OnScoreUpdated;

    public int CurrentScore { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        OnScoreUpdated?.Invoke(CurrentScore, 0); 
    }

    public void AddMove()
    {
        CurrentScore++;
        OnScoreUpdated?.Invoke(CurrentScore, 1); 
    }

    public void CalculateStars()
    {
        Debug.Log("⭐ 3 YILDIZ ALDIN! Toplam Hamle: " + CurrentScore);
    }
}