using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ShootingPower : MonoBehaviour
{
    [Header("Powerup")]
    [SerializeField] private bool hasPowerup;
    [SerializeField] public GameObject powerupIndicator;
    [SerializeField] private int powerUpDuration = 5;

    [Header("Fire")]
    [SerializeField] private InputAction fireAction;
    [SerializeField] private GameObject projectilePrefab;
    private Vector3 projectileOrigin;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireAction.Enable();
        projectileOrigin = new Vector3(0f, 1.2f, 0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (fireAction.triggered) 
        {
            Instantiate(projectilePrefab, transform.position + projectileOrigin, projectilePrefab.transform.rotation);
        }

        powerupIndicator.transform.position = transform.position + new Vector3(0, 0.2f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            // Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            // Vector3 awayFromPlayer =  collision.gameObject.transform.position - transform.position; 

            Destroy(collision.gameObject);
        }
    }

}
