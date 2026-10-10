using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";

    [Header("Scoring")]
    [SerializeField] private int pointsPerEnemy = 10;

    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }

    public event Action<int, int> ScoreChanged;

    private void Awake()
    {
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        CurrentScore = 0;

        ScoreChanged?.Invoke(CurrentScore, HighScore);
    }

    public void AddScore(int points)
    {
        if (points <= 0)
        {
            return;
        }

        CurrentScore += points;

        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;

            PlayerPrefs.SetInt(HighScoreKey, HighScore);
            PlayerPrefs.Save();
        }

        ScoreChanged?.Invoke(CurrentScore, HighScore);
    }

    public void AwardEnemyDefeated()
    {
        AddScore(pointsPerEnemy);
    }

    public void ResetCurrentScore()
    {
        CurrentScore = 0;
        ScoreChanged?.Invoke(CurrentScore, HighScore);
    }
}