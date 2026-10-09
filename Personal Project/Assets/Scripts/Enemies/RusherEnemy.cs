using UnityEngine;

public class RusherEnemy : Enemy
{
    [Header("Rusher Settings")]
    [SerializeField] private float meleeRange = 0.01f;

    [SerializeField] private float attackCooldown = 1f;
    private float nextAttackTime;

    protected override void PerformBehaviour()
    {
        if (IsPlayerInMeleeRange())
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
        Vector3 direction = player.position - transform.position;
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

    private void AttackPlayer()
    {
        if (Time.time < nextAttackTime)
        {
            return;
        }

        nextAttackTime = Time.time + attackCooldown;

        playerDamageable?.TakeDamage(1);
    }

    private bool IsPlayerInMeleeRange()
    {
        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        return distance <= meleeRange;
    }
}