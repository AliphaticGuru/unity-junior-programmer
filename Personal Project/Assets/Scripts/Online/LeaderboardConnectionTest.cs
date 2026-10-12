
using System;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;

public class LeaderboardConnectionTest : MonoBehaviour
{
    private const string LeaderboardId =
        "survival_highscores";

    private async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance
                    .SignInAnonymouslyAsync();
            }

            Debug.Log(
                "UGS connected. Player ID: " +
                AuthenticationService.Instance.PlayerId
            );

            // Temporary test score.
            await LeaderboardsService.Instance
                .AddPlayerScoreAsync(LeaderboardId, 100);

            Debug.Log("Test score submitted successfully.");

            var scores = await LeaderboardsService.Instance
                .GetScoresAsync(LeaderboardId);

            foreach (var entry in scores.Results)
            {
                Debug.Log(
                    $"Rank: {entry.Rank + 1}, " +
                    $"Player: {entry.PlayerId}, " +
                    $"Score: {entry.Score}"
                );
            }
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}
