using UnityEngine;

public class MoveToKill : MonoBehaviour
{
    public float speed = 20f;
    private float horizontalRangeX = 24f;
    private PlayerController playerController;
    private float projectileDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        // PlayerController player = playerObj.GetComponent<PlayerController>();
        playerController = FindAnyObjectByType<PlayerController>();
        if (playerController != null)
        {
            projectileDirection = playerController.Facing; // 180f or -180f both work perfectly
            // Vector3 newScale = transform.localScale;
            // newScale.x = Mathf.Abs(newScale.x) * projectileDirection;
            // transform.localScale = newScale;

            float zRotation = projectileDirection > 0f ? -90f : 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
            // transform.rotation = Quaternion.Euler(0f, 0f, projectileRotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * projectileDirection * Time.deltaTime * speed, Space.World);

        if (transform.position.x < -horizontalRangeX)
        {
            Destroy(gameObject);
            // Debug.Log("Game Over!");
        } 
        if (transform.position.x > horizontalRangeX)
        {
            Destroy(gameObject);
        }
    }

    // Detect collision with other objects
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
