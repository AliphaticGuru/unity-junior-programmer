using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Milestone #5: singleton survival director. Spawns enemy/pickup/obstacle
/// waves ahead of the player along the lane, tracks kills and survival
/// time as score, and reacts to PlayerLives.onPlayerDied. Put on an empty
/// "SurvivalManager" GameObject and wire the player's onPlayerDied event
/// to HandlePlayerDied in the Inspector (or via code in Start).
/// </summary>
public class SurvivalManager : MonoBehaviour
{
    public static SurvivalManager Instance { get; private set; }

    [Header("Spawn Points (place ahead of the lane, off-screen)")]
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private Transform[] pickupSpawnPoints;
    [SerializeField] private Transform[] obstacleSpawnPoints;

    [Header("Prefabs")]
    [SerializeField] private GameObject rangedEnemyPrefab;
    [SerializeField] private GameObject rusherEnemyPrefab;
    [SerializeField] private GameObject[] pickupPrefabs;
    [SerializeField] private GameObject[] obstaclePrefabs;

    [Header("Wave Tuning")]
    [SerializeField] private float baseEnemyInterval = 3f;
    [SerializeField] private float baseObstacleInterval = 5f;
    [SerializeField] private float basePickupInterval = 7f;
    [SerializeField] private float difficultyGrowthPerSecond = 0.02f;
    [SerializeField] private float maxDifficultyMultiplier = 3f;
    [SerializeField] [Range(0f, 1f)] private float rusherChance = 0.4f;

    public int KillCount { get; private set; }
    public float SurvivalTime { get; private set; }
    public bool IsGameOver { get; private set; }
    public float DifficultyMultiplier { get; private set; } = 1f;

    public UnityEvent<int> onKillCountChanged = new UnityEvent<int>();
    public UnityEvent<float> onSurvivalTimeChanged = new UnityEvent<float>();
    public UnityEvent onGameOver = new UnityEvent();

    private float enemyTimer, obstacleTimer, pickupTimer;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Update()
    {
        if (IsGameOver) return;

        SurvivalTime += Time.deltaTime;
        onSurvivalTimeChanged.Invoke(SurvivalTime);

        DifficultyMultiplier = Mathf.Min(maxDifficultyMultiplier, 1f + difficultyGrowthPerSecond * SurvivalTime);

        TickTimer(ref enemyTimer, baseEnemyInterval, SpawnEnemy);
        TickTimer(ref obstacleTimer, baseObstacleInterval, SpawnObstacle);
        TickTimer(ref pickupTimer, basePickupInterval, SpawnPickup);
    }

    private void TickTimer(ref float timer, float baseInterval, System.Action spawnAction)
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            spawnAction();
            timer = baseInterval / DifficultyMultiplier;
        }
    }

    private void SpawnEnemy()
    {
        if (enemySpawnPoints.Length == 0) return;
        var point = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
        var prefab = Random.value < rusherChance ? rusherEnemyPrefab : rangedEnemyPrefab;
        if (prefab != null) Instantiate(prefab, point.position, Quaternion.identity);
    }

    private void SpawnObstacle()
    {
        if (obstacleSpawnPoints.Length == 0 || obstaclePrefabs.Length == 0) return;
        var point = obstacleSpawnPoints[Random.Range(0, obstacleSpawnPoints.Length)];
        var prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        Instantiate(prefab, point.position, Quaternion.identity);
    }

    private void SpawnPickup()
    {
        if (pickupSpawnPoints.Length == 0 || pickupPrefabs.Length == 0) return;
        var point = pickupSpawnPoints[Random.Range(0, pickupSpawnPoints.Length)];
        var prefab = pickupPrefabs[Random.Range(0, pickupPrefabs.Length)];
        Instantiate(prefab, point.position, Quaternion.identity);
    }

    public void OnEnemyKilled()
    {
        KillCount++;
        onKillCountChanged.Invoke(KillCount);
    }

    public void HandlePlayerDied()
    {
        IsGameOver = true;
        SurvivalHighScoreStore.TrySave(SurvivalTime, KillCount);
        onGameOver.Invoke();
    }

    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}

/// <summary>
/// Milestone #5: local high-score persistence for best survival time / kills.
/// </summary>
public static class SurvivalHighScoreStore
{
    private const string TimeKey = "SidefireLastStand_BestTime";
    private const string KillsKey = "SidefireLastStand_BestKills";

    public static float GetBestTime() => PlayerPrefs.GetFloat(TimeKey, 0f);
    public static int GetBestKills() => PlayerPrefs.GetInt(KillsKey, 0);

    public static void TrySave(float time, int kills)
    {
        if (time > GetBestTime()) PlayerPrefs.SetFloat(TimeKey, time);
        if (kills > GetBestKills()) PlayerPrefs.SetInt(KillsKey, kills);
        PlayerPrefs.Save();
    }
}
