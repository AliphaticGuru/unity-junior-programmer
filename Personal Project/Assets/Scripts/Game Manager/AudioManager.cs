using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Player")]
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip shoot;
    [SerializeField] private AudioClip melee;
    [SerializeField] private AudioClip playerHit;
    [SerializeField] private AudioClip lifeLost;

    [Header("Gameplay")]
    [SerializeField] private AudioClip enemyDefeated;
    [SerializeField] private AudioClip powerupCollected;
    [SerializeField] private AudioClip waveStarted;
    [SerializeField] private AudioClip gameOver;

    [Header("UI")]
    [SerializeField] private AudioClip buttonClick;

    [Header("Settings")]
    [SerializeField, Range(0f, 1f)]
    private float sfxVolume = 0.7f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();
    }

    private void Play(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
            return;

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayJump() => Play(jump);
    public void PlayShoot() => Play(shoot);
    public void PlayMelee() => Play(melee);
    public void PlayPlayerHit() => Play(playerHit);
    public void PlayLifeLost() => Play(lifeLost);

    public void PlayEnemyDefeated() => Play(enemyDefeated);
    public void PlayPowerup() => Play(powerupCollected);
    public void PlayWaveStarted() => Play(waveStarted);
    public void PlayGameOver() => Play(gameOver);

    public void PlayButtonClick() => Play(buttonClick);
}
