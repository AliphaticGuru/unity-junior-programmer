using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("Base Enemy Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private int maxHealth = 3;

    protected Transform player;
    protected SurvivalManager survivalManager;

    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public float Speed => speed;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Start()
    {
        FindPlayer();
        FindSurvivalManager();

        speed = Mathf.Clamp(
            speed + survivalManager.enemySpeed,
            speed,
            survivalManager.enemySpeedCap
        );
    }

    protected virtual void Update()
    {
        if (player == null)
        {
            return;
        }

        PerformBehaviour();
    }

    protected abstract void PerformBehaviour();

    public virtual void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void FindSurvivalManager()
    {
        survivalManager = FindAnyObjectByType<SurvivalManager>();
    }
}

// public class Enemy : MonoBehaviour
// {
//     public float speed;
//     private float horizontalRangeX = 23f;
//     public SurvivalManager survivalManager;
//     private Rigidbody enemyRb;
//     private GameObject player;
//     Vector3 lookDir;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         enemyRb = GetComponent<Rigidbody>();
//         player = GameObject.Find("Player");
//         survivalManager = GameObject.Find("Survival Manager").GetComponent<SurvivalManager>();
//         // speed += survivalManager.enemySpeed;

//         speed = Mathf.Clamp(speed + survivalManager.enemySpeed, speed, survivalManager.enemySpeedCap);
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         Vector3 rawDir = player.transform.position - transform.position;

//         rawDir.y = 0f; // Ignore vertical difference

//         lookDir = rawDir.normalized; // Normalize the direction vector to get a unit vector
//         // enemyRb.AddForce(lookDir * speed * Time.deltaTime);
//         transform.Translate(lookDir * speed * Time.deltaTime);
//         // If an object goes out of bounds, destroy it
//         if (transform.position.x < -horizontalRangeX)
//         {
//             Destroy(gameObject);
//             // Debug.Log("Game Over!");
//         } 
//         if (transform.position.z > horizontalRangeX)
//         {
//             Destroy(gameObject);
//         }
//     }

//     // private void OnCollisionEnter(Collision collision)
//     // {
//     //     if (collision.gameObject.CompareTag("Player"))
//     //     {
//     //         // isOnGround = true;
//     //     }
//     // }
// }
