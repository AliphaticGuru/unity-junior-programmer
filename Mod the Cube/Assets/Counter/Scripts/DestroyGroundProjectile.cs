using UnityEngine;

public class DestroyGroundProjectile : MonoBehaviour
{
    [Header("Void Cleanup Settings")]
    public float killDepthY = -20f;       // Delete automatically if it drops below this height
    public float maxLifetimeSeconds = 8f; // Absolute safety cutoff timer in case it flies infinitely

    private void Start()
    {
        // Absolute fallback rule: automatically destroy this object after X seconds 
        // regardless of where it travels, preventing phantom memory leaks.
        Destroy(gameObject, maxLifetimeSeconds);
    }

    private void Update()
    {
        // Check every frame if the rock has plummeted past the boundaries into the void
        if (transform.position.y < killDepthY)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the tag "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
                    // NEW: Play a dull thud sound and spawn ground impact dust/dirt particles
        if (EffectsManager.Instance != null)
        {
            EffectsManager.Instance.PlaySound(EffectsManager.Instance.groundHitSFX, 0.6f);
            
            // Get the precise point of physical contact for the particle system
            Vector3 hitPoint = collision.contacts[0].point; 
            EffectsManager.Instance.SpawnParticles(
                EffectsManager.Instance.groundParticlesPrefab, 
                hitPoint, 
                Quaternion.LookRotation(collision.contacts[0].normal)
            );
        }
            
            // Destroy this projectile
            Destroy(gameObject);
        }
    }
}
