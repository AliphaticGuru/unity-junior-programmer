using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 100f;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    private InputSystem_Actions controls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
        
    // }

    void Awake()
    {
        controls = new InputSystem_Actions();
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        // controls.Player.Enable();
    }
    private void OnEnable()
    {
        controls.Player.Enable();
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();
        float forwardInput = moveInput.y;
        playerRb.AddForce(focalPoint.transform.forward * forwardInput * playerSpeed * Time.deltaTime);
    }
}
