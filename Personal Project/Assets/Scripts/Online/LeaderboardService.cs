
using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;

public class LeaderboardService : MonoBehaviour
{
    private const string LeaderboardId =
        "survival_highscores";

    private Task initializationTask;

    public Task InitializeAsync()
    {
        return initializationTask ??=
            InitializeInternalAsync();
    }

    private async Task InitializeInternalAsync()
    {
        try
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance
                    .SignInAnonymouslyAsync();
            }
        }
        catch (Exception exception)
        {
            Debug.LogWarning(
                $"Leaderboard initialization failed: {exception.Message}"
            );

            throw;
        }
    }

    public async Task<bool> SubmitScoreAsync(int score)
    {
        if (score < 0)
            return false;

        try
        {
            await InitializeAsync();

            await LeaderboardsService.Instance
                .AddPlayerScoreAsync(LeaderboardId, score);

            return true;
        }
        catch (Exception exception)
        {
            Debug.LogWarning(
                $"Score submission failed: {exception.Message}"
            );

            return false;
        }
    }

    public async Task<LeaderboardScoresPage>
        GetTopScoresAsync()
    {
        try
        {
            await InitializeAsync();

            return await LeaderboardsService.Instance
                .GetScoresAsync(
                    LeaderboardId,
                    new GetScoresOptions { Limit = 10 }
                );
        }
        catch (Exception exception)
        {
            Debug.LogWarning(
                $"Leaderboard retrieval failed: {exception.Message}"
            );

            return null;
        }
    }
}
