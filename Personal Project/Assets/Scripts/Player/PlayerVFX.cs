
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerPowerUp playerPowerUp;
    [SerializeField] private Rigidbody playerRigidbody;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem speedTrail;
    [SerializeField] private ParticleSystem meleeTrail;

    [Header("Settings")]
    [SerializeField] private float movementThreshold = 0.1f;

    private void Update()
    {
        UpdateSpeedTrail();
    }

    private void UpdateSpeedTrail()
    {
        if (playerPowerUp == null ||
            playerRigidbody == null ||
            speedTrail == null)
            return;

        bool isMoving =
            Mathf.Abs(playerRigidbody.linearVelocity.x)
            > movementThreshold;

        bool shouldEmit =
            playerPowerUp.HasPowerup && isMoving;

        if (shouldEmit)
        {
            if (!speedTrail.isEmitting)
                speedTrail.Play();
        }
        else
        {
            if (speedTrail.isEmitting)
                speedTrail.Stop(
                    true,
                    ParticleSystemStopBehavior.StopEmitting
                );
        }
    }

    public void PlayMeleeEffect()
    {
        if (meleeTrail == null)
            return;

        meleeTrail.Play();
    }

    private void OnDisable()
    {
        if (speedTrail != null)
            speedTrail.Stop(true,
                ParticleSystemStopBehavior.StopEmittingAndClear);

        if (meleeTrail != null)
            meleeTrail.Stop(true,
                ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
