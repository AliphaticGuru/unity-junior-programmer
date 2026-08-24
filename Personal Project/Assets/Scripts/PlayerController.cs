using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float speed = 6f;
    private float HorizontalBoundary = 23.0f;
    private float bottomBound = 0.5f;
    private Rigidbody playerRb;
    public float jumpForce;
    public bool isOnGround;

    public InputAction moveAction;
    
    public Vector2 moveInput;
    
    // public InputAction verticalInput;
    // public InputAction horizontalInput;
    public float gravityModifier;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        moveAction.Enable();
        // InvokeRepeating("MovePlayer", 0, 0.01f);
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

        // if player is on a ground/floor, jump upwards
        if (moveInput.y > 0 && isOnGround)
        {
            // float vertical = verticalInput.ReadValue<float>();
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        } 

        // if player is on ground/floor that is not the base floor, move downwards
        else if (moveInput.y < 0 && isOnGround)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);
            isOnGround = false;

            // if player verical position is less than the base floor level, reset it back to the base floor level
            if (transform.position.y < bottomBound)
            {
                transform.position = new Vector3(transform.position.x, bottomBound, transform.position.z);
                playerRb.AddForce(Vector3.up * 0.3f, ForceMode.Impulse);
                isOnGround = true;
            }
        }

        // controls players horizontal movement while staying in the designated boundary
        if (moveInput.x > 0)
        {
            // transform.Translate(Vector3.right * speed * Time.deltaTime);
            playerRb.AddForce(Vector3.right * speed * Time.deltaTime, ForceMode.Impulse);
        }

        if (moveInput.x < 0)
        {
            // transform.Translate(Vector3.left * speed );
            playerRb.AddForce(Vector3.left * speed * Time.deltaTime, ForceMode.Impulse);
        }
        
        if (playerPos.x >= HorizontalBoundary)
        {
            transform.position = new Vector3(HorizontalBoundary, transform.position.y, transform.position.z);
        }

        if (playerPos.x < -HorizontalBoundary)
        {
            transform.position = new Vector3(-HorizontalBoundary, transform.position.y, transform.position.z);
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
