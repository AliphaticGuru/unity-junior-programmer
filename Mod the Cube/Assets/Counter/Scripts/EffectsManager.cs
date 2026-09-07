using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance { get; private set; }

    [Header("Audio Source")]
    public AudioSource globalAudioSource;

    [Header("Sound Effects (SFX)")]
    public AudioClip launchSFX;
    public AudioClip targetHitSFX;
    public AudioClip groundHitSFX;

    [Header("Particle Prefabs")]
    public GameObject launchParticlesPrefab;   // Poof/dust at the pouch when fired
    public GameObject targetParticlesPrefab;   // Confetti/wood explosion when target breaks
    public GameObject groundParticlesPrefab;   // Dust puff on ground hit

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Auto-assign AudioSource if one exists on this object
        if (globalAudioSource == null) globalAudioSource = GetComponent<AudioSource>();
    }

    // Universal helper to play sound clips without clipping or interrupting each other
    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null && globalAudioSource != null)
        {
            globalAudioSource.PlayOneShot(clip, volume);
        }
    }

    // Universal helper to spawn visual particles and auto-destroy them to save memory
    public void SpawnParticles(GameObject prefab, Vector3 position, Quaternion rotation, float duration = 2f)
    {
        if (prefab != null)
        {
            GameObject spawnedParticles = Instantiate(prefab, position, rotation);
            Destroy(spawnedParticles, duration); // Keeps memory clean!
        }
    }
}
