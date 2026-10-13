using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;


    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("UI")]
    [SerializeField] private GameObject pauseButton;

    [Header("Game Over Scores")]
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private GameObject newRecordLabel;

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
        {
            pauseButton.SetActive(state == GameState.Playing);
        }

        
        if (state == GameState.GameOver)
        {
            if (scoreManager != null)
            {
                int finalScore = scoreManager.CurrentScore;
                int highScore = scoreManager.HighScore;

                finalScoreText.text = $"Final Score: {finalScore}";

                if (highScoreText != null)
                    highScoreText.text = $"High Score: {highScore}";

                if (newRecordLabel != null)
                {
                    bool isNewRecord =
                        finalScore > 0 && finalScore == highScore;

                    newRecordLabel.SetActive(isNewRecord);
                }
            }
            
            UpdateGameOverScores();
            
        }

    }

    private void UpdateGameOverScores()
    {
        if (scoreManager == null)
        {
            Debug.LogWarning(
                "GameUIController: ScoreManager is not assigned."
            );
            return;
        }

        if (finalScoreText != null)
        {
            finalScoreText.text =
                $"Final Score: {scoreManager.CurrentScore}";
        }

        if (highScoreText != null)
        {
            highScoreText.text =
                $"High Score: {scoreManager.HighScore}";
        }
    }

    public void OnPauseClicked()
    {
        gameManager.PauseGame();

        AudioManager.Instance?.PlayButtonClick();
    }

    public void OnResumeClicked()
    {
        gameManager.ResumeGame();

        AudioManager.Instance?.PlayButtonClick();
    }

    public void OnRestartClicked()
    {
        gameManager.RestartGame();

        AudioManager.Instance?.PlayButtonClick();
    }

    public void OnMainMenuClicked()
    {
        gameManager.ReturnToMainMenu();

       AudioManager.Instance?.PlayButtonClick(); 
    }
}
