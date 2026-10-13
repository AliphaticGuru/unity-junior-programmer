
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private GameObject powerupPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private SaveGameManager saveGameManager;

    [Header("Enemy Spawning")]
    [SerializeField] private float spawnRangeX = 24f;
    [SerializeField] private float spawnYMin = 0.5f;
    [SerializeField] private float spawnYMax = 10f;

    [Header("Wave Settings")]
    [SerializeField] private int startingEnemyCount = 1;
    [SerializeField] private int maxEnemiesPerWave = 15;
    [SerializeField] private float waveDelay = 3f;

    [Header("Difficulty")]
    [SerializeField] private float startingEnemySpeed = 1f;
    [SerializeField] private float enemySpeedIncrease = 0.2f;
    [SerializeField] private float enemySpeedCap = 3f;

    [Header("Player")]
    [SerializeField] private bool resetPlayerEachWave = false;
    [SerializeField] private Vector3 playerSpawnPosition =
        new Vector3(0f, 0.2f, -3f);


    public float EnemySpeedBonus {get; private set;}
    public float EnemySpeedCap => enemySpeedCap;


    public int CurrentWave { get; private set; }
    public int ActiveEnemyCount { get; private set; }
    public float CurrentEnemySpeed { get; private set; }

    public event Action<int> WaveStarted;
    public event Action<int> WaveCompleted;

    public event Action<int> WaveCountdownChanged;
    public event Action<float> DifficultyChanged;

    private readonly HashSet<Enemy> activeEnemies = new();

    private Coroutine waveRoutine;
    private bool waveInProgress;
    private bool isGameOver;

    private void Start()
    {
        CurrentEnemySpeed = startingEnemySpeed;
        EnemySpeedBonus = 0f;

        if (gameManager == null || player == null ||
            enemyPrefabs == null || enemyPrefabs.Count == 0)
        {
            Debug.LogError(
                "SurvivalManager has missing references.", this);
            enabled = false;
            return;
        }

        gameManager.StateChanged += HandleGameStateChanged;

        // Restore progress before spawning the first wave.
        if (saveGameManager != null)
            saveGameManager.InitializeRun();

        if (gameManager.CurrentState == GameState.Playing)
            StartNextWave();
    }

    private void OnDestroy()
    {
        if (gameManager != null)
            gameManager.StateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state != GameState.GameOver)
            return;

        isGameOver = true;

        if (waveRoutine != null)
        {
            StopCoroutine(waveRoutine);
            waveRoutine = null;
        }
    }

    private void StartNextWave()
    {
        if (isGameOver || waveInProgress ||
            waveRoutine != null)
            return;

        waveRoutine = StartCoroutine(BeginWave());
    }

    
    private IEnumerator BeginWave()
    {
        yield return null;
        
        if (CurrentWave > 0)
        {
            int countdown = Mathf.CeilToInt(waveDelay);

            while (countdown > 0)
            {
                if (isGameOver)
                {
                    waveRoutine = null;
                    yield break;
                }

                WaveCountdownChanged?.Invoke(countdown);

                yield return new WaitForSeconds(1f);

                countdown--;
            }

            WaveCountdownChanged?.Invoke(0);
        }

        if (isGameOver)
        {
            waveRoutine = null;
            yield break;
        }

        CurrentWave++;

        int enemiesToSpawn = Mathf.Min(
            startingEnemyCount + CurrentWave - 1,
            maxEnemiesPerWave
        );

        if (resetPlayerEachWave)
            ResetPlayerPosition();

        waveInProgress = true;

        SpawnEnemyWave(enemiesToSpawn);
        SpawnPowerupIfNeeded();

        WaveStarted?.Invoke(CurrentWave);

        AudioManager.Instance?.PlayWaveStarted();

        waveRoutine = null;

        if (ActiveEnemyCount == 0)
            CompleteWave();
    }


    private void SpawnEnemyWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject prefab = enemyPrefabs[
                UnityEngine.Random.Range(0, enemyPrefabs.Count)
            ];

            if (prefab == null)
                continue;

            GameObject instance = Instantiate(
                prefab,
                GenerateSpawnPosition(),
                prefab.transform.rotation
            );

            Enemy enemy = instance.GetComponent<Enemy>();

            if (enemy != null)
            {
                activeEnemies.Add(enemy);
                enemy.Defeated += HandleEnemyDefeated;
            }
            else
            {
                Debug.LogWarning(
                    "Spawned prefab has no Enemy component.",
                    instance
                );
            }
        }

        ActiveEnemyCount = activeEnemies.Count;
    }

    private void HandleEnemyDefeated(Enemy enemy)
    {
        if (enemy == null || !activeEnemies.Remove(enemy))
            return;

        enemy.Defeated -= HandleEnemyDefeated;

        ActiveEnemyCount = activeEnemies.Count;

        if (ActiveEnemyCount == 0 && waveInProgress)
            CompleteWave();
    }

    private void CompleteWave()
    {
        if (isGameOver || !waveInProgress)
            return;

        waveInProgress = false;

        WaveCompleted?.Invoke(CurrentWave);

        EnemySpeedBonus += enemySpeedIncrease;

        DifficultyChanged?.Invoke(EnemySpeedBonus);

        StartNextWave();
    }

    private Vector3 GenerateSpawnPosition()
    {
        return new Vector3(
            UnityEngine.Random.Range(-spawnRangeX, spawnRangeX),
            UnityEngine.Random.Range(spawnYMin, spawnYMax),
            -3f
        );
    }

    private void SpawnPowerupIfNeeded()
    {
        if (powerupPrefab == null)
            return;

        if (GameObject.FindGameObjectWithTag("Powerup") != null)
            return;

        Vector3 offset = new Vector3(-2f, 0.5f, 0f);

        Vector3 position = new Vector3(
            UnityEngine.Random.Range(-spawnRangeX, spawnRangeX),
            0f,
            -3f
        );

        Instantiate(
            powerupPrefab,
            position + offset,
            powerupPrefab.transform.rotation
        );
    }

    private void ResetPlayerPosition()
    {
        player.transform.position = playerSpawnPosition;

        if (player.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void RestoreProgress(int wave, float speedBonus)
    {
        CurrentWave = Mathf.Max(0, wave - 1);
        EnemySpeedBonus = Mathf.Max(0f, speedBonus);

        waveInProgress = false;
        isGameOver = false;
    }

}
