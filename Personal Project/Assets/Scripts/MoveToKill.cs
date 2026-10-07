using UnityEngine;

public class MoveToKill : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float horizontalRangeX = 24f;
    [SerializeField] private int damage = 1;

    private float projectileDirection;

    public void Initialize(float direction)
    {
        projectileDirection = direction;

        float zRotation =
            projectileDirection > 0f ? -90f : 90f;

        transform.rotation =
            Quaternion.Euler(0f, 0f, zRotation);
    }

    private void Update()
    {
        transform.Translate(
            Vector3.right *
            projectileDirection *
            speed *
            Time.deltaTime,
            Space.World
        );

        if (transform.position.x < -horizontalRangeX ||
            transform.position.x > horizontalRangeX)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player projectiles can only damage enemies.
        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        IDamageable damageable =
            other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}




// using UnityEngine;

// public class MoveToKill : MonoBehaviour
// {
//     public float speed = 20f;
//     private float horizontalRangeX = 24f;
//     private PlayerController playerController;
//     private float projectileDirection;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         // GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
//         // PlayerController player = playerObj.GetComponent<PlayerController>();
//         playerController = FindAnyObjectByType<PlayerController>();
//         if (playerController != null)
//         {
//             projectileDirection = playerController.Facing; // 180f or -180f both work perfectly

//             float zRotation = projectileDirection > 0f ? -90f : 90f;
//             transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         transform.Translate(Vector3.right * projectileDirection * Time.deltaTime * speed, Space.World);

//         if (transform.position.x < -horizontalRangeX)
//         {
//             Destroy(gameObject);
//             // Debug.Log("Game Over!");
//         } 
//         if (transform.position.x > horizontalRangeX)
//         {
//             Destroy(gameObject);
//         }
//     }

//     // Detect collision with other objects
//     void OnTriggerEnter(Collider other)
//     {
//         // Player bullets can only damage enemies.
//         if (!other.CompareTag("Enemy"))
//         {
//             return;
//         }

//         IDamageable damageable = other.GetComponent<IDamageable>();
        
//         if (damageable != null)
//         {
//             damageable.TakeDamage(1); // Assuming the projectile deals 1 damage
//         }

//         Destroy(gameObject);
//     }
// }
