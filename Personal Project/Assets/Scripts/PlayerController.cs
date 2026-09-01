using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Lane Movement")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float HorizontalBoundary = 22.5f;
    [SerializeField] private float bottomBound = 0f;
    [SerializeField] private float facing = 1f;
    public float Facing => facing;
    // [SerializeField] private GameObject focalPoint;
    private Rigidbody playerRb;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private bool isOnGround;
    [SerializeField] private float gravityModifier;    
    [SerializeField] public InputAction moveAction;
    [SerializeField] public InputAction jumpAction;
    
    public Vector2 moveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        // focalPoint = GameObject.Find("Focal Point");
        moveAction.Enable();
        jumpAction.Enable();
    }
    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }
    // Player controller using the x and y-components vector
    private void MovePlayer()
    {
        // Read the users horizontal and vertical keypress and assigned to moveInput variable
        moveInput = moveAction.ReadValue<Vector2>();                
        Vector3 playerPos = transform.position;
        if (moveInput.x != 0f)
        {
            facing = moveInput.x > 0f ? 1f : -1f;
        }

        // if player is on a ground/floor, jump upwards
        if (jumpAction.triggered && isOnGround)
        {
            // float vertical = verticalInput.ReadValue<float>();
            playerRb.AddForce(Vector3.up *  jumpForce, ForceMode.Impulse);
            isOnGround = false;
        } 

        // if player is on ground/floor that is not the base floor, move downwards
        if (moveInput.y < 0 && isOnGround)
        {
            playerPos = new Vector3(playerPos.x, playerPos.y - 1.5f, playerPos.z);
            isOnGround = false;
        }

        // if player verical position is less than the base floor level, reset it back to the base floor level
        if (playerPos.y < bottomBound)
        {
            playerPos = new Vector3(playerPos.x, bottomBound, playerPos.z);
            // playerRb.AddForce(Vector3.up * 0.3f, ForceMode.Impulse);
            isOnGround = true;
        }

        // controls players horizontal movement while staying in the designated boundary
        transform.Translate(transform.right * moveInput.x * speed * Time.deltaTime);

        // Only update rotation if the player is actively pressing a direction
        if (moveInput.x != 0)
        {
            float yRotation = moveInput.x > 0f ? 0f : -180f; // 180f or -180f both work perfectly
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        }
        
        if (playerPos.x >= HorizontalBoundary)
        {
            transform.position = new Vector3(HorizontalBoundary -1, transform.position.y, transform.position.z);
        }
        else if (playerPos.x <= -HorizontalBoundary)
        {
            transform.position = new Vector3(-HorizontalBoundary +1, transform.position.y, transform.position.z);
        }
    }

    // check if player is on a floor, then enable jump upward
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isOnGround = true;
        }
        if (collision.gameObject.CompareTag("Floor0"))
        {
            isOnGround = true;    
        }
    }
}
