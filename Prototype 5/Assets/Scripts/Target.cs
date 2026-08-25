// using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Target : MonoBehaviour
{
    public ParticleSystem explosionParticle;
    private Rigidbody targetRb;
    private GameManager gameManager;
    private float minSpeed = 12f;
    private float maxSpeed = 16f;
    private float maxTorque = 10f;
    private float xRange = 4f;
    private float ySpawnPos = -4f;
    public int pointValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(),RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPos();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }
    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && gameManager.isGameActive)
        {
            Debug.Log("Mouse was clicked");
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // If ray hit this enemy, destroy it
                if (hit.transform == transform)
                {
                    Destroy(gameObject);
                    Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
                    gameManager.UpdateScore(pointValue);
                }
            }
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DestroyZone")) 
        {
            Destroy(gameObject);
            if (!gameObject.CompareTag("Bad"))
            {
                gameManager.GameOver();
            }
        }  
    }
}
