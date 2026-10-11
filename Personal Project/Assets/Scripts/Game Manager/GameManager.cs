
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Scene Settings")]
    [SerializeField] private string mainMenuScene = "MainMenu";

    public GameState CurrentState { get; private set; }

    public event Action<GameState> StateChanged;

    private void Awake()
    {
        Time.timeScale = 1f;
        CurrentState = GameState.Playing;
    }

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.Died += HandlePlayerDeath;
    }

    private void Start()
    {
        if (playerHealth == null)
            Debug.LogError("PlayerHealth is not assigned.", this);

        StateChanged?.Invoke(CurrentState);
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.Died -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        SetState(GameState.GameOver);
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        SetState(GameState.Playing);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    private void SetState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;

        Time.timeScale =
            newState == GameState.Playing ? 1f : 0f;

        StateChanged?.Invoke(CurrentState);
    }
}
