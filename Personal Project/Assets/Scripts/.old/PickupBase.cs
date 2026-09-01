using UnityEngine;

/// <summary>
/// Milestone #4: pickup base + two concrete types. Attach the concrete
/// component to each prefab along with a trigger Collider.
/// </summary>
public abstract class PickupBase : MonoBehaviour
{
    [SerializeField] private GameObject collectVfxPrefab;
    [SerializeField] private AudioClip collectSfx;

    private void OnTriggerEnter2D(Collider2D other) => TryCollect(other.gameObject);
    private void OnTriggerEnter(Collider other) => TryCollect(other.gameObject);

    private void TryCollect(GameObject obj)
    {
        if (!obj.CompareTag("Player")) return;
        if (!Apply(obj)) return;

        if (collectVfxPrefab != null) Instantiate(collectVfxPrefab, transform.position, Quaternion.identity);
        if (collectSfx != null) AudioSource.PlayClipAtPoint(collectSfx, transform.position);
        Destroy(gameObject);
    }

    /// <returns>true if the pickup was actually used (e.g. ammo wasn't already full)</returns>
    protected abstract bool Apply(GameObject player);
}




