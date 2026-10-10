using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;

    private static readonly int MovingHash =
        Animator.StringToHash("Moving");

    private static readonly int AttackHash =
        Animator.StringToHash("Attack");

    private static readonly int ShootHash =
        Animator.StringToHash("Shoot");

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError(
                $"{name}: No Animator found in children.",
                this
            );
        }
    }

    public void SetMoving(bool isMoving)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(MovingHash, isMoving);
    }

    public void PlayAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger(AttackHash);
        }
    }

    public void PlayShoot()
    {
        Debug.Log($"{name}: PlayShoot() called");

        if (animator == null)
        {
            Debug.LogError($"{name}: Animator is missing!");
            return;
        }

        animator.ResetTrigger(ShootHash);
        animator.SetTrigger(ShootHash);
        
        // if (animator != null)
        // {
        //     animator.SetTrigger(ShootHash);
        // }
    }
}