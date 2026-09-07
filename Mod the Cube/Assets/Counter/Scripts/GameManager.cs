using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Playing, Paused, GameOver }
public enum Difficulty { Easy, Medium, Hard }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Text References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI difficultyText;

    [Header("UI Panel Windows")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverFinalScoreText;

    [Header("Game Settings")]
    public float gameDurationSeconds = 60f;

    [Header("Main Menu Configuration")]
    public GameObject mainMenuPanel;
    
    // Gameplay variables
    private int score = 0;
    private float timeRemaining;
    private GameState currentState = GameState.Playing;
    private Difficulty currentDifficulty;
    public Difficulty GetActiveDifficulty() { return currentDifficulty; }


    // References to your existing scripts to disable input when paused/gameover
    private SlingshotInteractions interactionsScript;
    private SlingshotCamera cameraScript;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        timeRemaining = gameDurationSeconds;
        
        // Find existing scripts automatically
        interactionsScript = FindFirstObjectByType<SlingshotInteractions>();
        cameraScript = FindFirstObjectByType<SlingshotCamera>();

        UpdateUI();
        // ResumeGame(); // Ensure time is moving at start

            // Force the main menu window open when scene boots up
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
            currentState = GameState.Paused;
            Time.timeScale = 0f; // Freeze time completely until a choice is selected
            TogglePlayerControls(false); // Stop pulling the elastic band behind menu screens
        }
        else
        {
            ResumeGame();
        }
    }

    void Update()
    {
        // Toggle Pause with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Playing) PauseGame();
            else if (currentState == GameState.Paused) ResumeGame();
        }

        if (currentState == GameState.Playing)
        {
            UpdateTimer();
        }
    }

    // A new universal launcher helper routine for our buttons to activate
    public void StartGameWithDifficulty(int difficultyIndex)
    {
        // Apply setting profile definitions (0=Easy, 1=Medium, 2=Hard)
        SetDifficulty(difficultyIndex);
        
        // Close the interface panel overlay and start the timer
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        
        // Inform target in scene to update their initial scales before physics boot up
        Counter targetInstance = FindFirstObjectByType<Counter>();
        if (targetInstance != null) targetInstance.RespawnAtRandomLocation();

        ResumeGame();
    }

    // -----------------------------------------
    // CORE SCORE LOGIC
    // -----------------------------------------
    public void IncrementScore()
    {
        if (currentState != GameState.Playing) return;

        // Give bonus points based on difficulty setting
        int pointsAwarded = 1;
        if (currentDifficulty == Difficulty.Medium) pointsAwarded = 2;
        else if (currentDifficulty == Difficulty.Hard) pointsAwarded = 3;

        score += pointsAwarded;
        UpdateUI();
    }

    void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            TriggerGameOver();
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (timerText != null) timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining) + "s";
        // if (difficultyText != null) difficultyText.text = "Difficulty: " + currentDifficulty.ToString();
    }

    // -----------------------------------------
    // DIFFICULTY MODIFIERS
    // -----------------------------------------
    public void SetDifficulty(int difficultyIndex)
    {
        // 0 = Easy, 1 = Medium, 2 = Hard
        currentDifficulty = (Difficulty)difficultyIndex;
        UpdateUI();

        // Adjust your game behaviors depending on difficulty here!
        if (interactionsScript != null)
        {
            if (currentDifficulty == Difficulty.Easy) interactionsScript.releaseForce = 50f;
            if (currentDifficulty == Difficulty.Medium) interactionsScript.releaseForce = 40f; // Harder pull/less force
            if (currentDifficulty == Difficulty.Hard) interactionsScript.releaseForce = 30f;   // Weakest raw force, requires precise aiming
        }
    }

    // Call this when the mouse enters a button area
    public void OnHoverDifficultyButton(int difficultyIndex)
    {
        Difficulty hoveredDiff = (Difficulty)difficultyIndex;
        
        if (difficultyText != null)
        {
            // Add a visual indicator (like color or stars) to show it's a real-time preview
            string previewColor = hoveredDiff == Difficulty.Easy ? "<color=green>" : hoveredDiff == Difficulty.Medium ? "<color=yellow>" : "<color=red>";
            currentDifficulty = hoveredDiff; // Temporarily preview the difficulty
            difficultyText.text = "Difficulty: " + previewColor + hoveredDiff.ToString() + "</color>";
        }
    }

    // Call this when the mouse exits a button area
    public void OnLeaveDifficultyButton()
    {
        // Revert text to reflect what is ACTUALLY selected (or a default prompt)
        if (difficultyText != null)
        {
            difficultyText.text = "Difficulty: " + currentDifficulty.ToString();
        }
    }


    // -----------------------------------------
    // STATE MANAGERS (Pause, Resume, Game Over)
    // -----------------------------------------
    public void PauseGame()
    {
        currentState = GameState.Paused;
        Time.timeScale = 0f; // Freezes all physics matrices and delta loops
        if (pausePanel != null) pausePanel.SetActive(true);
        TogglePlayerControls(false);
    }

    public void ResumeGame()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f; // Resumes normal flow
        if (pausePanel != null) pausePanel.SetActive(false);
        TogglePlayerControls(true);
    }

    void TriggerGameOver()
    {
        currentState = GameState.GameOver;
        Time.timeScale = 0f; // Stop everything
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (gameOverFinalScoreText != null) gameOverFinalScoreText.text = "Final Score: " + score;
        TogglePlayerControls(false);
    }

    void TogglePlayerControls(bool state)
    {
        // Block player from dragging slingshot or orbiting camera when menus are active
        if (interactionsScript != null) interactionsScript.enabled = state;
        if (cameraScript != null) cameraScript.enabled = state;
    }

    // -----------------------------------------
    // UI BUTTON HOOKS
    // -----------------------------------------
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        // Adjust if you build a separate home title screen index scene layout later
        SceneManager.LoadScene(0); 
    }
}