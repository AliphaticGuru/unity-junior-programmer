using UnityEngine;

/// <summary>
/// Milestone #1: 2.5D side-view movement. The player moves along the
/// world X axis (the "lane"), with a fixed Z depth, plus jump and a
/// dodge-dash with brief invincibility frames. Attach to the player
/// along with a CharacterController.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class SideScrollPlayerController : MonoBehaviour
{
    [Header("Lane Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float laneZ = -3f;          // locked depth for the side-view
    [SerializeField] private float laneMinX = -23f;
    [SerializeField] private float laneMaxX = 23f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Dodge")]
    [SerializeField] private float dodgeSpeed = 14f;
    [SerializeField] private float dodgeDuration = 0.2f;
    [SerializeField] private float dodgeCooldown = 0.8f;
    [SerializeField] private float dodgeInvincibilityTime = 0.25f;

    private CharacterController cc;
    private float verticalVelocity;
    private float dodgeTimer;
    private float dodgeCooldownTimer;
    private bool isDodging;
    private float facing = 1f; // +1 right, -1 left — used by shooting/melee for aim direction

    public bool IsInvincible { get; private set; }
    public float Facing => facing;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(input) > 0.01f) facing = Mathf.Sign(input);

        HandleDodgeInput();

        Vector3 move;
        if (isDodging)
        {
            move = new Vector3(facing * dodgeSpeed, 0f, 0f);
            dodgeTimer -= Time.deltaTime;
            if (dodgeTimer <= 0f) EndDodge();
        }
        else
        {
            move = new Vector3(input * moveSpeed, 0f, 0f);
        }

        // Ground check + jump
        if (cc.isGrounded)
        {
            verticalVelocity = -1f; // small downward stick force
            if (Input.GetButtonDown("Jump"))
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        move.y = verticalVelocity;

        cc.Move(move * Time.deltaTime);

        // Clamp to lane bounds and lock depth (in case of any Z drift from collisions)
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, laneMinX, laneMaxX);
        pos.z = laneZ;
        transform.position = pos;

        // Face the direction of travel (visual only — swap for an Animator flip if using sprites/rig)
        transform.rotation = Quaternion.Euler(0f, facing > 0 ? 90f : -90f, 0f);

        dodgeCooldownTimer -= Time.deltaTime;
    }

    private void HandleDodgeInput()
    {
        if (isDodging) return;
        // "Fire2" is a default Input Manager axis (Left Ctrl / gamepad), so this
        // works out of the box without adding a custom "Dodge" axis.
        bool pressed = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButtonDown("Fire2");
        if (pressed && dodgeCooldownTimer <= 0f)
        {
            isDodging = true;
            dodgeTimer = dodgeDuration;
            dodgeCooldownTimer = dodgeCooldown;
            IsInvincible = true;
            Invoke(nameof(EndInvincibility), dodgeInvincibilityTime);
        }
    }

    private void EndDodge() => isDodging = false;
    private void EndInvincibility() => IsInvincible = false;
}
