using UnityEngine;
using System.Collections;

/// <summary>
/// Milestone #3: stays at a preferred range and fires a telegraphed shot
/// at the player on a cooldown. The telegraph gives the player a fair
/// window to dodge.
/// </summary>
public class RangedEnemy : EnemyBase
{
    [Header("Ranged Behavior")]
    [SerializeField] private float preferredRange = 8f;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float fireCooldown = 2f;
    [SerializeField] private float telegraphTime = 0.4f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Renderer telegraphIndicator; // e.g. a glowing decal/eye — swap material or enable during telegraph

    private float cooldownTimer;
    private bool isTelegraphing;

    protected override void Awake()
    {
        base.Awake();
        cooldownTimer = Random.Range(0f, fireCooldown); // stagger initial shots across enemies
    }

    private void Update()
    {
        if (player == null || isTelegraphing) return;

        float distance = Mathf.Abs(player.position.x - transform.position.x);
        float dirX = DirectionToPlayerX();

        // Maintain preferred range: approach if too far, back off if too close.
        if (distance > preferredRange + 0.5f)
            transform.position += new Vector3(dirX * moveSpeed * Time.deltaTime, 0f, 0f);
        else if (distance < preferredRange - 0.5f)
            transform.position -= new Vector3(dirX * moveSpeed * Time.deltaTime, 0f, 0f);

        // Face the player.
        transform.rotation = Quaternion.Euler(0f, dirX > 0 ? 90f : -90f, 0f);

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            cooldownTimer = fireCooldown;
            StartCoroutine(TelegraphThenFire());
        }
    }

    private IEnumerator TelegraphThenFire()
    {
        isTelegraphing = true;
        if (telegraphIndicator != null) telegraphIndicator.enabled = true;

        yield return new WaitForSeconds(telegraphTime);

        if (telegraphIndicator != null) telegraphIndicator.enabled = false;
        isTelegraphing = false;

        if (player == null) yield break;

        // Simple hitscan toward the player's current position at fire time —
        // the telegraph delay is what makes this dodgeable, not travel time.
        var playerLives = player.GetComponent<PlayerLives>();
        Vector3 dir = new Vector3(DirectionToPlayerX(), 0f, 0f);
        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 30f, playerLayer))
        {
            playerLives?.TakeDamage(attackDamage);
        }
    }
}
