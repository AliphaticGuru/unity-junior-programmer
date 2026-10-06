using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Ranged Settings")]
    [SerializeField] private float attackRange = 5f;

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
        Vector3 direction = player.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            direction.Normalize();

            transform.Translate(
                direction * Speed * Time.deltaTime,
                Space.World
            );
        }
    }

    private bool IsPlayerInAttackRange()
    {
        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        return distance <= attackRange;
    }

    private void AttackPlayer()
    {
        // Ranged attack will be implemented later.
    }
}