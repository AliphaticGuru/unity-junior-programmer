using System.Collections;
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action StatsChanged;

    [Header("Player Health")]
    [SerializeField] private int maxHealthPerLife = 3;

    [Header("Player Lives")]
    [SerializeField] private int maxLives = 3;

    [Header("Respawn")]
    [SerializeField] private float invulnerabilityDuration = 2f;
    [SerializeField] private Vector3 respawnPosition = new Vector3(0f, 0.2f, -3f);


    private int currentHealth;
    private bool isInvulnerable;
    private int currentLives;

    private bool isDead;

    public bool IsDead => isDead;

    public bool IsInvulnerable => isInvulnerable;

    public int CurrentLives => currentLives;
    public int MaxLives => maxLives;

    public int CurrentHealth => currentHealth;
    public int MaxHealthPerLife => maxHealthPerLife;

    private void Awake()
    {
        currentHealth = maxHealthPerLife;
        currentLives = maxLives;

        StatsChanged?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0 || isInvulnerable)
        {
            return;
        }

        currentHealth = Mathf.Max(0, currentHealth - damage);

        Debug.Log(
            $"Player Health: {currentHealth}/{maxHealthPerLife}"
        );

        if (currentHealth <= 0)
        {
            LoseLife();
            return;
        }

        StatsChanged?.Invoke();
    }

    private void LoseLife()
    {
        if (isDead || currentLives <= 0)
        {
            return;
        }

        currentLives = Mathf.Max(0, currentLives - 1);

        Debug.Log($"Player lost a life. Lives remaining: {currentLives}");

        if (currentLives == 0)
        {
            Die();
            return;
        }
        
        currentHealth = maxHealthPerLife;
        StatsChanged?.Invoke();

        Respawn();
    }

    private void Respawn()
    {
        transform.position = respawnPosition;

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        StartCoroutine(InvulnerabilityTimer());
    }

    private IEnumerator InvulnerabilityTimer()
    {
        isInvulnerable = true;

        yield return new WaitForSeconds(invulnerabilityDuration);

        isInvulnerable = false;
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        currentHealth = 0;
        currentLives = 0;

        StatsChanged?.Invoke();
        
        Debug.Log("Game Over!");

        gameObject.SetActive(false);
    }
}