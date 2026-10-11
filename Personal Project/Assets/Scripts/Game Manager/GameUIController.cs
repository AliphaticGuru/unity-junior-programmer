
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScoreManager scoreManager;

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("UI")]
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private TMP_Text finalScoreText;

    private void OnEnable()
    {
        if (gameManager != null)
            gameManager.StateChanged += UpdateUI;
    }

    private void Start()
    {
        if (gameManager != null)
            UpdateUI(gameManager.CurrentState);
    }

    private void OnDisable()
    {
        if (gameManager != null)
            gameManager.StateChanged -= UpdateUI;
    }

    private void UpdateUI(GameState state)
    {
        pausePanel.SetActive(state == GameState.Paused);
        gameOverPanel.SetActive(state == GameState.GameOver);

        if (pauseButton != null)
            pauseButton.SetActive(state == GameState.Playing);

        if (state == GameState.GameOver && scoreManager != null)
        {
            finalScoreText.text =
                $"Final Score: {scoreManager.CurrentScore}";
        }
    }

    public void OnPauseClicked()
    {
        gameManager.PauseGame();
    }

    public void OnResumeClicked()
    {
        gameManager.ResumeGame();
    }

    public void OnRestartClicked()
    {
        gameManager.RestartGame();
    }

    public void OnMainMenuClicked()
    {
        gameManager.ReturnToMainMenu();
    }
}
