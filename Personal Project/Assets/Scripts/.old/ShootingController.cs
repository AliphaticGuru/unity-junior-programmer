using UnityEngine;

/// <summary>
/// Milestone #2: fires a raycast shot along the player's facing direction.
/// Raycast (instead of physical projectiles) keeps hit-detection simple
/// and reliable for an MVP; swap for pooled projectiles later if you want
/// visible travel time. Attach to the player, alongside
/// SideScrollPlayerController.
/// </summary>
[RequireComponent(typeof(SideScrollPlayerController))]
public class ShootingController : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private float fireRate = 0.25f;      // seconds between shots
    [SerializeField] private float range = 25f;
    [SerializeField] private int damage = 1;
    [SerializeField] private LayerMask hittableLayers;
    [SerializeField] private Transform muzzlePoint;

    [Header("Ammo")]
    [SerializeField] private int maxAmmo = 12;
    [SerializeField] private float reloadTime = 1.2f;

    [Header("FX")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioClip fireSfx;
    [SerializeField] private AudioClip reloadSfx;
    [SerializeField] private AudioClip emptySfx;

    public int CurrentAmmo { get; private set; }
    public bool IsReloading { get; private set; }

    // HUD subscribes here instead of polling.
    public System.Action<int, int> onAmmoChanged; // (current, max)

    private SideScrollPlayerController playerController;
    private float fireCooldown;
    private AudioSource audioSource;

    private void Awake()
    {
        playerController = GetComponent<SideScrollPlayerController>();
        audioSource = GetComponent<AudioSource>();
        CurrentAmmo = maxAmmo;
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (IsReloading) return;

        if (Input.GetKeyDown(KeyCode.R) && CurrentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        bool wantsToFire = Input.GetButton("Fire1");
        if (wantsToFire && fireCooldown <= 0f)
            TryFire();
    }

    private void TryFire()
    {
        if (CurrentAmmo <= 0)
        {
            if (emptySfx != null) audioSource.PlayOneShot(emptySfx);
            return;
        }

        fireCooldown = fireRate;
        CurrentAmmo--;
        onAmmoChanged?.Invoke(CurrentAmmo, maxAmmo);

        Vector3 origin = muzzlePoint != null ? muzzlePoint.position : transform.position;
        Vector3 dir = new Vector3(playerController.Facing, 0f, 0f);

        if (Physics.Raycast(origin, dir, out RaycastHit hit, range, hittableLayers))
        {
            var damageable = hit.collider.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage);
        }

        if (muzzleFlash != null) muzzleFlash.Play();
        if (fireSfx != null) audioSource.PlayOneShot(fireSfx);
    }

    public void AddAmmo(int amount)
    {
        CurrentAmmo = Mathf.Min(maxAmmo, CurrentAmmo + amount);
        onAmmoChanged?.Invoke(CurrentAmmo, maxAmmo);
    }

    private System.Collections.IEnumerator Reload()
    {
        IsReloading = true;
        if (reloadSfx != null) audioSource.PlayOneShot(reloadSfx);
        yield return new WaitForSeconds(reloadTime);
        CurrentAmmo = maxAmmo;
        onAmmoChanged?.Invoke(CurrentAmmo, maxAmmo);
        IsReloading = false;
    }
}

/// <summary>
/// Anything that can take damage: enemies, perishable obstacles, etc.
/// </summary>
public interface IDamageable
{
    void TakeDamage(int amount);
}
