using UnityEngine;

/// <summary>
/// Milestone #3: charges straight at the player and attacks on contact
/// range — the archetype that pressures the player into using melee or
/// backing off with a dodge.
/// </summary>
public class RusherEnemy : EnemyBase
{
    [SerializeField] private float moveSpeed = 4.5f;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int attackDamage = 1;

    private float cooldownTimer;

    private void Update()
    {
        if (player == null) return;

        float distance = Mathf.Abs(player.position.x - transform.position.x);
        float dirX = DirectionToPlayerX();
        transform.rotation = Quaternion.Euler(0f, dirX > 0 ? 90f : -90f, 0f);

        if (distance > attackRange)
        {
            transform.position += new Vector3(dirX * moveSpeed * Time.deltaTime, 0f, 0f);
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                cooldownTimer = attackCooldown;
                player.GetComponent<PlayerLives>()?.TakeDamage(attackDamage);
            }
        }
    }
}
