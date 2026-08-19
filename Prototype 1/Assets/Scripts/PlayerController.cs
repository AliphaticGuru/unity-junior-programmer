using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // movement tuning (editable in inspector)
    public float turnSpeed = 100f;
    public float speed = 5.0f;

    // Input System action exposed in Inspector for binding (WASD/Arrow key)
    public InputAction moveAction;

    // Current input value (x = left/right, y = forward/back), kept private for internal use
    private Vector2 moveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Enable the MoveAction so it starts reading input values from the keyboard or gamepad
        moveAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // Read the 2D vector from the MoveAction (x : horizontal, y : vertical) and store it in moveInput
        moveInput = moveAction.ReadValue<Vector2>();
        
        // move the vehicle forward/back along Z axis using the y component
        transform.Translate(Vector3.forward * Time.deltaTime * speed * moveInput.y);

        // turn the vehicle left/right along Y axis using the x component
        transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * moveInput.x);
    }
}
