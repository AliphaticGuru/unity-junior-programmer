using UnityEngine;

/// <summary>
/// Milestone #3: short-range melee strike, meant to one- or two-shot
/// enemies that get close — the "risk it for ammo savings" option.
/// Attach to the player alongside SideScrollPlayerController.
/// </summary>
[RequireComponent(typeof(SideScrollPlayerController))]
public class MeleeController : MonoBehaviour
{
    [SerializeField] private float meleeRange = 1.5f;
    [SerializeField] private int meleeDamage = 2;
    [SerializeField] private float meleeCooldown = 0.5f;
    [SerializeField] private LayerMask hittableLayers;
    [SerializeField] private AudioClip meleeSfx;
    [SerializeField] private GameObject hitVfxPrefab;

    private SideScrollPlayerController playerController;
    private AudioSource audioSource;
    private float cooldownTimer;

    private void Awake()
    {
        playerController = GetComponent<SideScrollPlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F) && cooldownTimer <= 0f)
            TryMelee();
    }

    private void TryMelee()
    {
        cooldownTimer = meleeCooldown;

        Vector3 origin = transform.position;
        Vector3 dir = new Vector3(playerController.Facing, 0f, 0f);

        // OverlapSphere at the strike point catches enemies without needing
        // exact facing precision — friendlier for an MVP than a tight raycast.
        Vector3 strikePoint = origin + dir * (meleeRange * 0.5f);
        Collider[] hits = Physics.OverlapSphere(strikePoint, meleeRange * 0.5f, hittableLayers);

        bool connected = false;
        foreach (var col in hits)
        {
            var damageable = col.GetComponent<IDamageable>();
            if (damageable == null) continue;
            damageable.TakeDamage(meleeDamage);
            connected = true;
        }

        if (connected && hitVfxPrefab != null)
            Instantiate(hitVfxPrefab, strikePoint, Quaternion.identity);
        if (meleeSfx != null) audioSource.PlayOneShot(meleeSfx);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize melee range in the editor.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
