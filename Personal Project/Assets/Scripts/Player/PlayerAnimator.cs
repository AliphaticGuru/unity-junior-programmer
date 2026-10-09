using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();

        if (animator == null)
        {
            Debug.LogError("PlayerAnimator: No Animator found in children.");
        }
    }

    private void Update()
    {
        UpdateMovementAnimation();
    }

    private void UpdateMovementAnimation()
    {
        if (playerController == null)
        {
            return;
        }

        float horizontalInput =
            Mathf.Abs(playerController.moveInput.x);

        animator.SetFloat("Speed", horizontalInput);
    }

    public void PlayShoot()
    {
        if (animator == null)
        {
            return;
        }
        animator.SetTrigger("Shoot");
    }

    public void PlayMelee()
    {
        if (animator == null)
        {
            return;
        }

        animator.SetTrigger("Melee");
    }
}