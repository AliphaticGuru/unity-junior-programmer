using UnityEngine;

public class RusherEnemy : Enemy
{
    [Header("Rusher Settings")]
    [SerializeField] private float meleeRange = 1.5f;

    protected override void PerformBehaviour()
    {
        MoveTowardsPlayer();
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

    private bool IsPlayerInMeleeRange()
    {
        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        return distance <= meleeRange;
    }
}