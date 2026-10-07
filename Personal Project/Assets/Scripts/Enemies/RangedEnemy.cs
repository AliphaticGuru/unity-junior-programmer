using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Ranged Settings")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Vector3 projectileSpawnOffset =
        new Vector3(0f, 1.2f, 0f);

    private float nextAttackTime;

    protected override void PerformBehaviour()
    {
        if (IsPlayerInAttackRange())
        {
            AttackPlayer();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction =
            player.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
        {
            return;
        }

        direction.Normalize();

        transform.Translate(
            direction * Speed * Time.deltaTime,
            Space.World
        );
    }

    private bool IsPlayerInAttackRange()
    {
        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        return distance <= attackRange;
    }

    private void AttackPlayer()
    {
        if (Time.time < nextAttackTime)
        {
            return;
        }

        if (projectilePrefab == null)
        {
            return;
        }

        nextAttackTime =
            Time.time + attackCooldown;

        Vector3 spawnPosition =
            transform.position +
            projectileSpawnOffset;

        GameObject projectile =
            Instantiate(
                projectilePrefab,
                spawnPosition,
                Quaternion.identity
            );

        EnemyProjectile enemyProjectile =
            projectile.GetComponent<EnemyProjectile>();

        if (enemyProjectile != null)
        {
            enemyProjectile.Initialize(
                Facing
            );
        }
    }
}