using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Milestone #3: each life is one hit of tolerance (arcade-style) rather
/// than a gradual health bar — see design doc note. Health pickups grant
/// a bonus life up to maxLives instead of restoring HP.
/// Attach to the player alongside SideScrollPlayerController.
/// </summary>
[RequireComponent(typeof(SideScrollPlayerController))]
public class PlayerLives : MonoBehaviour, IDamageable
{
    [SerializeField] private int startingLives = 3;
    [SerializeField] private int maxLives = 5;
    [SerializeField] private float postHitInvincibility = 1f;

    public int CurrentLives { get; private set; }
    public bool IsDead { get; private set; }

    public UnityEvent<int> onLivesChanged = new UnityEvent<int>();
    public UnityEvent onPlayerDied = new UnityEvent();

    private SideScrollPlayerController controller;
    private float invincibilityTimer;

    private void Awake()
    {
        controller = GetComponent<SideScrollPlayerController>();
        CurrentLives = startingLives;
    }

    private void Update()
    {
        if (invincibilityTimer > 0f) invincibilityTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Called by enemy attacks (ranged shots or rusher melee). Any
    /// "amount" is treated the same — one hit costs exactly one life.
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (IsDead) return;
        if (controller.IsInvincible) return;   // mid-dodge
        if (invincibilityTimer > 0f) return;   // just got hit

        invincibilityTimer = postHitInvincibility;
        CurrentLives--;
        onLivesChanged.Invoke(CurrentLives);

        if (CurrentLives <= 0)
        {
            IsDead = true;
            onPlayerDied.Invoke();
        }
    }

    public void AddLife(int amount = 1)
    {
        CurrentLives = Mathf.Min(maxLives, CurrentLives + amount);
        onLivesChanged.Invoke(CurrentLives);
    }
}
