using UnityEngine;

/// <summary>
/// Shared base for enemy archetypes — handles HP, death VFX/SFX, and
/// score/kill-count notification. Concrete behavior (ranged vs rusher)
/// lives in the subclasses below.
/// </summary>
public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] protected int maxHealth = 2;
    [SerializeField] private GameObject deathVfxPrefab;
    [SerializeField] private AudioClip deathSfx;

    protected int currentHealth;
    protected Transform player;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    protected virtual void Die()
    {
        if (deathVfxPrefab != null) Instantiate(deathVfxPrefab, transform.position, Quaternion.identity);
        if (deathSfx != null) AudioSource.PlayClipAtPoint(deathSfx, transform.position);
        SurvivalManager.Instance?.OnEnemyKilled();
        Destroy(gameObject);
    }

    protected float DirectionToPlayerX()
    {
        if (player == null) return 0f;
        return Mathf.Sign(player.position.x - transform.position.x);
    }
}
