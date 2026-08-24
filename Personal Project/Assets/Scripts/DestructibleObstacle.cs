using UnityEngine;

/// <summary>
/// Milestone #4: a perishable obstacle (crate/barrel) that blocks the
/// lane until shot or meleed, and can optionally drop a pickup. Attach
/// to the obstacle prefab with a Collider (non-trigger, so it also
/// physically blocks movement/shots until destroyed).
/// </summary>
public class DestructibleObstacle : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 1;
    [SerializeField] private GameObject debrisVfxPrefab;
    [SerializeField] private AudioClip breakSfx;
    [SerializeField] private GameObject[] possibleDropPrefabs; // ammo/life pickups
    [SerializeField] [Range(0f, 1f)] private float dropChance = 0.35f;

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) Break();
    }

    private void Break()
    {
        if (debrisVfxPrefab != null) Instantiate(debrisVfxPrefab, transform.position, Quaternion.identity);
        if (breakSfx != null) AudioSource.PlayClipAtPoint(breakSfx, transform.position);

        if (possibleDropPrefabs.Length > 0 && Random.value <= dropChance)
        {
            var drop = possibleDropPrefabs[Random.Range(0, possibleDropPrefabs.Length)];
            Instantiate(drop, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
