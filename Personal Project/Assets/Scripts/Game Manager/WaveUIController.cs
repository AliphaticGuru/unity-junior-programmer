
using System.Collections;
using TMPro;
using UnityEngine;

public class WaveUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SurvivalManager survivalManager;
    [SerializeField] private GameManager gameManager;

    [Header("UI")]
    [SerializeField] private TMP_Text announcementText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text difficultyText;

    [Header("Settings")]
    [SerializeField] private float announcementDuration = 1.5f;

    private Coroutine announcementRoutine;

    private void OnEnable()
    {
        if (survivalManager != null)
        {
            survivalManager.WaveStarted += HandleWaveStarted;
            survivalManager.WaveCompleted += HandleWaveCompleted;
            survivalManager.WaveCountdownChanged += HandleCountdown;
            survivalManager.DifficultyChanged += HandleDifficulty;
        }

        if (gameManager != null)
            gameManager.StateChanged += HandleGameState;
    }

    private void Start()
    {
        announcementText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
        difficultyText.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (survivalManager != null)
        {
            survivalManager.WaveStarted -= HandleWaveStarted;
            survivalManager.WaveCompleted -= HandleWaveCompleted;
            survivalManager.WaveCountdownChanged -= HandleCountdown;
            survivalManager.DifficultyChanged -= HandleDifficulty;
        }

        if (gameManager != null)
            gameManager.StateChanged -= HandleGameState;
    }

    private void HandleWaveStarted(int wave)
    {
        countdownText.gameObject.SetActive(false);
        ShowAnnouncement($"WAVE {wave}");
    }

    private void HandleWaveCompleted(int wave)
    {
        ShowAnnouncement($"WAVE {wave} COMPLETE!");
    }

    private void HandleCountdown(int seconds)
    {
        if (seconds <= 0)
        {
            countdownText.gameObject.SetActive(false);
            return;
        }

        countdownText.gameObject.SetActive(true);
        countdownText.text = $"Next wave in {seconds}";
    }

    private void HandleDifficulty(float bonus)
    {
        difficultyText.gameObject.SetActive(true);
        difficultyText.text = $"Enemy Speed +{bonus:0.0}";
    }

    private void HandleGameState(GameState state)
    {
        if (state != GameState.GameOver)
            return;

        if (announcementRoutine != null)
        {
            StopCoroutine(announcementRoutine);
            announcementRoutine = null;
        }

        announcementText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
        difficultyText.gameObject.SetActive(false);
    }

    private void ShowAnnouncement(string message)
    {
        if (announcementRoutine != null)
            StopCoroutine(announcementRoutine);

        announcementRoutine = StartCoroutine(
            DisplayAnnouncement(message)
        );
    }

    private IEnumerator DisplayAnnouncement(string message)
    {
        announcementText.text = message;
        announcementText.gameObject.SetActive(true);

        yield return new WaitForSeconds(announcementDuration);

        announcementText.gameObject.SetActive(false);
        announcementRoutine = null;
    }
}
