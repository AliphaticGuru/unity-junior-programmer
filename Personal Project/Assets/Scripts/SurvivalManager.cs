using System.Collections.Generic;
using UnityEngine;

public class SurvivalManager : MonoBehaviour
{
    [Header("Object Prefabs")]
    [SerializeField] private List<GameObject> enemyPrefab;
    [SerializeField] private GameObject powerupPrefab;
    [SerializeField] private GameObject player;


    [Header("Spawn Enemy")]
    [SerializeField] private float spawnRangeX = 24.0f;
    [SerializeField] private float spawnYMin = 0.5f;
    [SerializeField] private float spawnYMax = 10f;

    [Header("Wave Data")]
    [SerializeField] private int enemyCount;
    [SerializeField] private int waveCount;
    [SerializeField] public float enemySpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveCount);
            waveCount++;
            enemySpeed += 0.1f;
        }

        Vector3 powerupSpawnOffset = new Vector3(-2f, 0.5f, 0f);

        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
        {
            Instantiate(powerupPrefab, GeneratePowerup() + powerupSpawnOffset, powerupPrefab.transform.rotation);
        }
    }
    Vector3 GenerateSpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float yPos = Random.Range(spawnYMin, spawnYMax);
        return new Vector3(xPos, yPos, -3f);
    }
    Vector3 GeneratePowerup()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        return new Vector3(xPos, 0, -3f);
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int index = Random.Range(0, enemyPrefab.Count);
            Instantiate(enemyPrefab[index], GenerateSpawnPosition(), enemyPrefab[index].transform.rotation);
        }
        ResetPlayerPosition();
    }
    void ResetPlayerPosition ()
    {
        player.transform.position = new Vector3(0, 0.2f, -3f);
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
