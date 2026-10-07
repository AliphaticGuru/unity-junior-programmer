using System.Collections;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    [Header("Powerup")]
    [SerializeField] private GameObject powerupIndicator;
    [SerializeField] private int powerUpDuration = 5;

    private bool hasPowerup;

    public bool HasPowerup => hasPowerup;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Powerup"))
        {
            return;
        }

        Destroy(other.gameObject);

        hasPowerup = true;

        if (powerupIndicator != null)
        {
            powerupIndicator.SetActive(true);
        }

        StartCoroutine(PowerupCooldown());
    }

    private IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);

        hasPowerup = false;

        if (powerupIndicator != null)
        {
            powerupIndicator.SetActive(false);
        }
    }

    private void Update()
    {
        if (powerupIndicator != null)
        {
            powerupIndicator.transform.position =
                transform.position + new Vector3(0f, 0.2f, 0f);
        }
    }
}