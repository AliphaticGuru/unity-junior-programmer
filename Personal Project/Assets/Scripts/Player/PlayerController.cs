using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Lane Movement")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float HorizontalBoundary = 24f;
    [SerializeField] private float bottomBound = 0f;
    [SerializeField] private float facing = 1f;

    public float Facing => facing;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] public bool isOnGround;
    [SerializeField] private float gravityModifier = 1f;

    [Header("Input")]
    [SerializeField] public InputAction moveAction;
    [SerializeField] public InputAction jumpAction;

    [Header("Powerup")]
    [SerializeField] private PlayerPowerUp playerPowerUp;

    private Rigidbody playerRb;
    private GameManager gameManager;

    public Vector2 moveInput;

    private bool jumpRequested;
    private bool dropRequested;

    private float EffectiveMoveSpeed =>
        speed * (
            playerPowerUp != null
                ? playerPowerUp.SpeedMultiplier
                : 1f
        );

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    private void Update()
    {
        if (gameManager == null ||
            gameManager.CurrentState != GameState.Playing)
        {
            moveInput = Vector2.zero;
            jumpRequested = false;
            dropRequested = false;
            return;
        }

        ReadInput();
        UpdateFacing();
    }

    private void FixedUpdate()
    {
        if (gameManager == null ||
            gameManager.CurrentState != GameState.Playing)
            return;

        ApplyHorizontalMovement();
        ApplyJump();
        ApplyDrop();
        ApplyExtraGravity();
        ApplyBoundaries();
    }

    private void ReadInput()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        if (jumpAction.WasPressedThisFrame() && isOnGround)
            jumpRequested = true;

        if (moveInput.y < -0.5f && isOnGround)
            dropRequested = true;
    }

    public void RestoreFacing(float direction)
    {
        facing = direction >= 0f ? 1f : -1f;

        transform.rotation = Quaternion.Euler(
            0f,
            facing > 0f ? 0f : 180f,
            0f
        );
    }

    private void UpdateFacing()
    {
        if (Mathf.Abs(moveInput.x) < 0.01f)
            return;

        facing = moveInput.x > 0f ? 1f : -1f;

        float yRotation = facing > 0f ? 0f : 180f;

        transform.rotation =
            Quaternion.Euler(0f, yRotation, 0f);
    }

    private void ApplyHorizontalMovement()
    {
        Vector3 velocity = playerRb.linearVelocity;

        velocity.x = moveInput.x * EffectiveMoveSpeed;

        playerRb.linearVelocity = velocity;
    }

    private void ApplyJump()
    {
        if (!jumpRequested)
            return;

        jumpRequested = false;

        if (!isOnGround)
            return;

        playerRb.AddForce(
            Vector3.up * jumpForce,
            ForceMode.Impulse
        );
        
        AudioManager.Instance?.PlayJump();

        isOnGround = false;
    }

    private void ApplyDrop()
    {
        if (!dropRequested)
            return;

        dropRequested = false;

        if (!isOnGround)
            return;

        Vector3 position = playerRb.position;

        position.y -= 1.5f;

        playerRb.MovePosition(position);

        isOnGround = false;
    }

    private void ApplyExtraGravity()
    {
        if (gravityModifier <= 1f)
            return;

        Vector3 extraGravity =
            Physics.gravity * (gravityModifier - 1f);

        playerRb.AddForce(
            extraGravity,
            ForceMode.Acceleration
        );
    }

    private void ApplyBoundaries()
    {
        Vector3 position = playerRb.position;
        Vector3 velocity = playerRb.linearVelocity;

        float minX = -HorizontalBoundary + 1f;
        float maxX = HorizontalBoundary - 1f;

        float clampedX = Mathf.Clamp(
            position.x,
            minX,
            maxX
        );

        if (!Mathf.Approximately(position.x, clampedX))
        {
            position.x = clampedX;

            playerRb.position = position;

            velocity.x = 0f;
            playerRb.linearVelocity = velocity;
        }

        if (position.y < bottomBound)
        {
            position.y = bottomBound;

            playerRb.position = position;

            if (velocity.y < 0f)
            {
                velocity.y = 0f;
                playerRb.linearVelocity = velocity;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") ||
            collision.gameObject.CompareTag("Floor0"))
        {
            isOnGround = true;
        }
    }
}




// using System.Collections;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerController : MonoBehaviour
// {
//     [Header("Lane Movement")]
//     [SerializeField] private float speed = 6f;
//     [SerializeField] private float HorizontalBoundary = 24.0f;
//     [SerializeField] private float bottomBound = 0f;
//     [SerializeField] private float facing = 1f;
//     public float Facing => facing;
//     // [SerializeField] private GameObject focalPoint;
//     private Rigidbody playerRb;
//     private GameManager gameManager;

//     [Header("Jump")]
//     [SerializeField] private float jumpForce = 10f;
//     [SerializeField] public bool isOnGround;
//     [SerializeField] private float gravityModifier;    
//     [SerializeField] public InputAction moveAction;
//     [SerializeField] public InputAction jumpAction;
//     [SerializeField] private PlayerPowerUp playerPowerUp;

//     private float EffectiveMoveSpeed =>
//     speed * (
//         playerPowerUp != null
//             ? playerPowerUp.SpeedMultiplier
//             : 1f
//     );
    
//     public Vector2 moveInput;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Awake()
//     {
//         gameManager = FindAnyObjectByType<GameManager>();
//     }
//     void Start()
//     {
//         playerRb = GetComponent<Rigidbody>();
//         Physics.gravity *= gravityModifier;
//         // focalPoint = GameObject.Find("Focal Point");
//         moveAction.Enable();
//         jumpAction.Enable();
//     }
//     // Update is called once per frame
//     void Update()
//     {
//         MovePlayer();
//     }
//     // Player controller using the x and y-components vector
//     private void MovePlayer()
//     {
//         if (gameManager.CurrentState != GameState.Playing)
//         {
//             return;
//         }
//         // Read the users horizontal and vertical keypress and assigned to moveInput variable
//         moveInput = moveAction.ReadValue<Vector2>();                
//         Vector3 playerPos = transform.position;
//         if (moveInput.x != 0f)
//         {
//             facing = moveInput.x > 0f ? 1f : -1f;
//         }

//         // if player is on a ground/floor, jump upwards
//         if (jumpAction.triggered && isOnGround)
//         {
//             // float vertical = verticalInput.ReadValue<float>();
//             playerRb.AddForce(Vector3.up *  jumpForce, ForceMode.Impulse);
//             isOnGround = false;
//         } 

//         // if player is on ground/floor that is not the base floor, move downwards
//         if (moveInput.y < 0 && isOnGround)
//         {
//             playerPos = new Vector3(playerPos.x, playerPos.y - 1.5f, playerPos.z);
//             isOnGround = false;
//         }

//         // if player verical position is less than the base floor level, reset it back to the base floor level
//         if (playerPos.y < bottomBound)
//         {
//             playerPos = new Vector3(playerPos.x, bottomBound, playerPos.z);
//             // playerRb.AddForce(Vector3.up * 0.3f, ForceMode.Impulse);
//             isOnGround = true;
//         }

//         // controls players horizontal movement while staying in the designated boundary
//         transform.Translate(transform.right * moveInput.x * EffectiveMoveSpeed * Time.deltaTime);

//         // playerRb.AddForce(Vector3.right * moveInput.x * EffectiveMoveSpeed * Time.deltaTime, ForceMode.Impulse);

//         // Only update rotation if the player is actively pressing a direction
//         if (moveInput.x != 0)
//         {
//             float yRotation = moveInput.x > 0f ? 0f : -180f; // 180f or -180f both work perfectly
//             transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
//         }
        
//         if (playerPos.x >= HorizontalBoundary)
//         {
//             transform.position = new Vector3(HorizontalBoundary -1, transform.position.y, transform.position.z);
//         }
//         else if (playerPos.x <= -HorizontalBoundary)
//         {
//             transform.position = new Vector3(-HorizontalBoundary +1, transform.position.y, transform.position.z);
//         }
//     }

//     // check if player is on a floor, then enable jump upward
//     private void OnCollisionEnter(Collision collision)
//     {
//         if (collision.gameObject.CompareTag("Floor"))
//         {
//             isOnGround = true;
//         }
//         if (collision.gameObject.CompareTag("Floor0"))
//         {
//             isOnGround = true;    
//         }
//     }
// }
