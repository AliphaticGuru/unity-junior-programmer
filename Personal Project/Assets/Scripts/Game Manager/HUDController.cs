using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [Header("Gameplay References")]
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("UI References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text livesText;

    private void OnEnable()
    {
        if (scoreManager != null)
        {
            scoreManager.ScoreChanged += UpdateScore;
        }

        if (playerHealth != null)
        {
            playerHealth.StatsChanged += UpdatePlayerStats;
        }
    }

    private void Start()
    {
        // Initial refresh because the managers may have
        // initialized before this component subscribed.
        RefreshHUD();
    }

    private void OnDisable()
    {
        if (scoreManager != null)
        {
            scoreManager.ScoreChanged -= UpdateScore;
        }

        if (playerHealth != null)
        {
            playerHealth.StatsChanged -= UpdatePlayerStats;
        }
    }

    private void RefreshHUD()
    {
        if (scoreManager != null)
        {
            UpdateScore(
                scoreManager.CurrentScore,
                scoreManager.HighScore
            );
        }

        UpdatePlayerStats();
    }

    private void UpdateScore(int score, int highScore)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }

        if (highScoreText != null)
        {
            highScoreText.text = $"HighScore: {highScore}";
        }
    }

    private void UpdatePlayerStats()
    {
        if (playerHealth == null)
        {
            return;
        }

        if (healthText != null)
        {
            healthText.text =
                $"Health: {playerHealth.CurrentHealth} / " +
                $"{playerHealth.MaxHealthPerLife}";
        }

        if (livesText != null)
        {
            livesText.text =
                $"Lives: {playerHealth.CurrentLives} / " +
                $"{playerHealth.MaxLives}";
        }
    }
}