using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 1;

    private float direction;

    public void Initialize(float facingDirection)
    {
        direction = facingDirection;

        // Rotate projectile to match horizontal direction.
        float yRotation = direction > 0f ? -90f : 90f;

        transform.rotation = Quaternion.Euler(
            0f,
            yRotation,
            0f
        );

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(
            Vector3.right *
            direction *
            speed *
            Time.deltaTime,
            Space.World
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
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