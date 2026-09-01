using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private float horizontalRangeX = 23f;
    public SurvivalManager survivalManager;
    private Rigidbody enemyRb;
    private GameObject player;
    Vector3 lookDir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        survivalManager = GameObject.Find("Survival Manager").GetComponent<SurvivalManager>();
        speed += survivalManager.enemySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        lookDir = (player.transform.position - transform.position).normalized;
        // enemyRb.AddForce(lookDir * speed * Time.deltaTime);
        transform.Translate(lookDir * speed * Time.deltaTime);
        // If an object goes out of bounds, destroy it
        if (transform.position.x < -horizontalRangeX)
        {
            Destroy(gameObject);
            // Debug.Log("Game Over!");
        } 
        if (transform.position.z > horizontalRangeX)
        {
            Destroy(gameObject);
        }
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         // isOnGround = true;
    //     }
    // }
}
