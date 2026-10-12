
using System;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private SurvivalManager survivalManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Rigidbody playerRigidbody;

    private JsonSaveRepository repository;

    public bool HasSave => repository.Exists;

    private void Awake()
    {
        repository = new JsonSaveRepository();
    }

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

    // Called by SurvivalManager.Start() before wave spawning.
    public void InitializeRun()
    {
        if (!GameSession.ContinueRequested)
            return;

        GameSession.ClearRequest();

        try
        {
            GameSaveData data = repository.Load();

            if (data == null)
            {
                Debug.LogWarning(
                    "No saved game found. Starting new game."
                );
                return;
            }

            RestoreGame(data);

            Debug.Log(
                $"Game restored at Wave {data.wave}."
            );
        }
        catch (Exception exception)
        {
            Debug.LogError(
                $"Failed to load game: {exception.Message}"
            );
        }
    }

    public void SaveGame()
    {
        if (gameManager == null ||
            (gameManager.CurrentState != GameState.Playing &&
            gameManager.CurrentState != GameState.Paused))
        {
            return;
        }

        if (scoreManager == null ||
            playerHealth == null ||
            survivalManager == null ||
            playerController == null ||
            playerRigidbody == null)
        {
            Debug.LogError(
                "SaveGameManager has missing references."
            );
            return;
        }

        if (playerHealth.IsDead)
            return;

        GameSaveData data = new GameSaveData
        {
            score = scoreManager.CurrentScore,
            wave = Mathf.Max(1, survivalManager.CurrentWave),
            health = playerHealth.CurrentHealth,
            lives = playerHealth.CurrentLives,
            enemySpeedBonus =
                survivalManager.EnemySpeedBonus,
            playerPosition = playerRigidbody.position,
            playerFacing = playerController.Facing
        };

        try
        {
            repository.Save(data);
            Debug.Log("Game saved successfully.");
        }
        catch (Exception exception)
        {
            Debug.LogError(
                $"Failed to save game: {exception.Message}"
            );
        }
    }

    private void RestoreGame(GameSaveData data)
    {
        if (data.wave < 1 ||
            data.health < 1 ||
            data.lives < 1)
        {
            throw new InvalidOperationException(
                "Save contains invalid player progress."
            );
        }

        scoreManager.RestoreScore(data.score);

        playerHealth.RestoreState(
            data.health,
            data.lives
        );

        survivalManager.RestoreProgress(
            data.wave,
            data.enemySpeedBonus
        );

        playerRigidbody.position = data.playerPosition;
        playerRigidbody.linearVelocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;

        // Restore the visual orientation.
        float facing = data.playerFacing >= 0f ? 1f : -1f;

        playerController.RestoreFacing(facing);
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state != GameState.GameOver)
            return;

        // A completed run cannot be continued.
        repository.Delete();
    }
}
