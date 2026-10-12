
using System;
using System.Collections;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    [Header("Powerup")]
    [SerializeField] private GameObject powerupIndicator;
    [SerializeField, Min(0.1f)]
    private float powerUpDuration = 6f;

    [SerializeField, Min(1f)]
    private float speedMultiplier = 3f;

    private Coroutine powerupRoutine;
    private bool hasPowerup;

    public bool HasPowerup => hasPowerup;

    public float SpeedMultiplier =>
        hasPowerup ? speedMultiplier : 1f;

    public event Action PowerupChanged;

    private void Awake()
    {
        if (powerupIndicator != null)
            powerupIndicator.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Powerup"))
            return;

        Destroy(other.gameObject);

        ActivatePowerup();
    }

    private void ActivatePowerup()
    {
        if (powerupRoutine != null)
            StopCoroutine(powerupRoutine);

        hasPowerup = true;

        if (powerupIndicator != null)
            powerupIndicator.SetActive(true);

        PowerupChanged?.Invoke();

        powerupRoutine = StartCoroutine(
            PowerupCooldown()
        );
    }

    private IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);

        powerupRoutine = null;
        DeactivatePowerup();
    }

    private void DeactivatePowerup()
    {
        if (!hasPowerup)
            return;

        hasPowerup = false;

        if (powerupIndicator != null)
            powerupIndicator.SetActive(false);

        PowerupChanged?.Invoke();
    }

    private void OnDisable()
    {
        if (powerupRoutine != null)
        {
            StopCoroutine(powerupRoutine);
            powerupRoutine = null;
        }

        DeactivatePowerup();
    }

    private void LateUpdate()
    {
        if (powerupIndicator != null)
        {
            powerupIndicator.transform.position =
                transform.position + new Vector3(0f, 0.2f, 0f);
        }
    }
}
