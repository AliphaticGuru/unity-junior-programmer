
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Unity.Services.Leaderboards.Models;

public class LeaderboardController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private LeaderboardService leaderboardService;

    [Header("UI")]
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private TMP_Text leaderboardText;
    [SerializeField] private TMP_Text statusText;

    private bool scoreSubmitted;

    private void OnEnable()
    {
        if (gameManager != null)
            gameManager.StateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        if (gameManager != null)
            gameManager.StateChanged -= HandleGameStateChanged;
    }

    private void Start()
    {
        if (leaderboardPanel != null)
            leaderboardPanel.SetActive(false);
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.GameOver && !scoreSubmitted)
        {
            scoreSubmitted = true;
            _ = SubmitFinalScoreAsync();
        }
    }

    private async Task SubmitFinalScoreAsync()
    {
        if (scoreManager == null || leaderboardService == null)
            return;

        int finalScore = scoreManager.CurrentScore;

        bool success = await leaderboardService
            .SubmitScoreAsync(finalScore);

        if (success)
            Debug.Log($"Final score submitted: {finalScore}");
        else
            Debug.LogWarning("Final score could not be submitted.");
    }

    public async void OpenLeaderboard()
    {
        if (leaderboardPanel == null ||
            leaderboardService == null ||
            leaderboardText == null ||
            statusText == null)
            return;

        leaderboardPanel.SetActive(true);

        statusText.text = "Loading leaderboard...";
        leaderboardText.text = "";

        try
        {
            LeaderboardScoresPage page =
                await leaderboardService.GetTopScoresAsync();

            if (page == null)
            {
                statusText.text = "Leaderboard unavailable.";
                return;
            }

            statusText.text = "";
            leaderboardText.text = "";

            if (page.Results.Count == 0)
            {
                statusText.text = "No scores yet.";
                return;
            }

            foreach (var entry in page.Results)
            {
                string playerName = string.IsNullOrEmpty(entry.PlayerName)
                    ? "Player"
                    : entry.PlayerName;

                leaderboardText.text +=
                    $"{entry.Rank + 1}. {playerName}  -  {entry.Score:0}\n";
            }
        }
        catch (Exception exception)
        {
            Debug.LogWarning(exception.Message);
            statusText.text = "Could not load leaderboard.";
        }
    }

    public void CloseLeaderboard()
    {
        if (leaderboardPanel != null)
            leaderboardPanel.SetActive(false);
    }
}
