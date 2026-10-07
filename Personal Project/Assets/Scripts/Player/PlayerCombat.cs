using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private InputAction attackAction;

    [Header("Melee")]
    [SerializeField] private float meleeRange = 1.5f;
    [SerializeField] private int meleeDamage = 1;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Ranged")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Vector3 projectileOrigin = new Vector3(0f, 1.2f, 0f);

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        attackAction.Enable();
    }

    private void OnDisable()
    {
        attackAction.Disable();
    }

    private void Update()
    {
        if (attackAction.triggered)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        IDamageable target = FindMeleeTarget();

        if (target != null)
        {
            PerformMeleeAttack(target);
        }
        else
        {
            PerformRangedAttack();
        }
    }

    private IDamageable FindMeleeTarget()
    {
        Vector3 attackCenter =
            transform.position +
            transform.right * meleeRange;

        Collider[] hitTargets = Physics.OverlapSphere(
            attackCenter,
            meleeRange,
            enemyLayer
        );

        float closestDistance = Mathf.Infinity;
        IDamageable closestTarget = null;

        foreach (Collider target in hitTargets)
        {
            IDamageable damageable =
                target.GetComponent<IDamageable>();

            if (damageable == null)
            {
                continue;
            }

            float distance =
                Vector3.Distance(transform.position, target.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = damageable;
            }
        }

        return closestTarget;
    }

    private void PerformMeleeAttack(IDamageable target)
    {
        target.TakeDamage(meleeDamage);

        Debug.Log("Player performed melee attack.");
    }

    private void PerformRangedAttack()
    {
        if (projectilePrefab == null)
        {
            return;
        }

        GameObject projectile = Instantiate(
            projectilePrefab,
            transform.position + projectileOrigin,
            projectilePrefab.transform.rotation
        );

        MoveToKill projectileMovement =
            projectile.GetComponent<MoveToKill>();

        if (projectileMovement != null)
        {
            projectileMovement.Initialize(
                playerController.Facing
            );
        }

        Debug.Log("Player performed ranged attack.");
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 attackCenter =
            transform.position +
            transform.right * meleeRange;

        Gizmos.DrawWireSphere(
            attackCenter,
            meleeRange
        );
    }
}