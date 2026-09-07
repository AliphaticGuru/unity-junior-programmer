using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    // public Text CounterText;

    // private int count = 0;
    [Header("Spawn Boundaries")]
    public float minX = -40f;
    public float maxX = 2f;
    public float minZ = -25f;
    public float maxZ = 30f;

    [Header("Difficulty Tuning")]
    public float easyScale = 1.5f;
    public float mediumScale = 1.0f;
    public float hardScale = 0.5f;
    public float movementWidth = 3f; // Distance it moves left/right

    // Keeping a fixed Y position so the target doesn't spawn underground or floating in the air
    private float fixedY; 
    private Vector3 spawnCenter;
    private float moveSpeed = 0f;
    private float randomOffsetTime;

    private void Start()
    {
        // count = 0;
        // Remember its initial height (Y) from where you placed it on your terrain
        fixedY = transform.position.y;

        randomOffsetTime = Random.Range(0f, 100f); // Prevents multiple targets from moving in perfect sync
        // Spawn at a random location right when the game starts
        RespawnAtRandomLocation();
    }

    private void Update()
    {
        // If the target has a movement speed, pace it back and forth over time
        if (moveSpeed > 0f)
        {
            float displacement = Mathf.Sin((Time.time + randomOffsetTime) * moveSpeed) * movementWidth;
            transform.position = new Vector3(spawnCenter.x + displacement, fixedY, spawnCenter.z);
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     count += 1;
    //     CounterText.text = "Count : " + count;
    // }

    // Inside your existing target script where a hit is verified:
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Automatically find our manager instance and safely tell it to track points!
            if (GameManager.Instance != null)
            {
                GameManager.Instance.IncrementScore();
            }

            // NEW: Trigger satisfying target break sound and explosion debris particles
            if (EffectsManager.Instance != null)
            {
                EffectsManager.Instance.PlaySound(EffectsManager.Instance.targetHitSFX, 1f);
                EffectsManager.Instance.SpawnParticles(
                    EffectsManager.Instance.targetParticlesPrefab, 
                    transform.position, 
                    Quaternion.identity
                );
            }

            // 2. Instead of deleting the target permanently, relocate it somewhere fresh!
            RespawnAtRandomLocation();
            
            // Destroys target or triggers collapse logic
            // Destroy(gameObject); 
            Destroy(collision.gameObject); // Destroy the projectile as well
        }
    }
    public void RespawnAtRandomLocation()
    {
        // Generate random 3D points inside your designated structural grid coordinates
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        spawnCenter = new Vector3(randomX, fixedY, randomZ);
        transform.position = spawnCenter;

        // 2. Safely capture the active game difficulty profile layout
        if (GameManager.Instance != null)
        {
            ApplyDifficultyModifiers();
        }
    }

    private void ApplyDifficultyModifiers()
    {
        // Fetch difficulty index from central manager string/enum state logic
        // Assuming SlingshotGameManager difficulty is set up correctly
        Difficulty currentDiff = GameManager.Instance.GetActiveDifficulty();

        switch (currentDiff)
        {
            case Difficulty.Easy:
                transform.localScale = Vector3.one * easyScale;
                moveSpeed = 0f; // Completely stationary target
                break;

            case Difficulty.Medium:
                transform.localScale = Vector3.one * mediumScale;
                moveSpeed = 2f; // Moderately slow shifting pacing motion
                break;

            case Difficulty.Hard:
                transform.localScale = Vector3.one * hardScale;
                moveSpeed = 5f; // Incredibly tiny target, zooming fast side-to-side!
                break;
        }
    }

}
