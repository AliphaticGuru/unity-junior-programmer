using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerX : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public GameObject titleScreen;
    public Button restartButton; 

    public List<GameObject> targetPrefabs;

    private int score;
    private float spawnRate = 1.5f;
    public bool isGameActive;

    private float spaceBetweenSquares = 2.5f; 
    private float minValueX = -3.75f; //  x value of the center of the left-most square
    private float minValueY = -3.75f; //  y value of the center of the bottom-most square

    [Header("Timer Settings")]
    [SerializeField] private float timeRemaining = 60f; // Set initial time in seconds
    [SerializeField] private bool timerIsActive = true;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI timerText; // Reference to UI Text element


    // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
    void Update()
    {
        if (timerIsActive && timeRemaining > 0)
            {
                // timeRemaining -= Time.deltaTime;
                UpdateDisplay(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsActive = false;
                UpdateDisplay(timeRemaining);
                OnTimerEnd();
            }
    }
    void UpdateDisplay(float timeToDisplay)
    {
        // Prevents negative numbers from showing briefly
        if (timeToDisplay < 0) timeToDisplay = 0; 

        // Calculate whole minutes and remaining seconds
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeRemaining -= Time.deltaTime;
        // Updates UI text with padded zeros (e.g., 01:05)
        timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    // Trigger actions when the timer finishes
    void OnTimerEnd()
    {
        // Add your custom game logic here (e.g., Game Over, Load Next Scene)
        GameOver();
    }
    
    public void StartGame(int difficulty)
    {
        isGameActive = true;
        timerIsActive = true;
        spawnRate /= difficulty;
        StartCoroutine(SpawnTarget());
        score = 0;
        UpdateScore(0);
        titleScreen.gameObject.SetActive(false);
    }

    // While game is active spawn a random target
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targetPrefabs.Count);

            Instantiate(targetPrefabs[index], RandomSpawnPosition(), targetPrefabs[index].transform.rotation);            
        }
    }
    // Formats float seconds into an easy-to-read MM:SS string
    

    // Generate a random spawn position based on a random index from 0 to 3
    Vector3 RandomSpawnPosition()
    {
        float spawnPosX = minValueX + (RandomSquareIndex() * spaceBetweenSquares);
        float spawnPosY = minValueY + (RandomSquareIndex() * spaceBetweenSquares);

        Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);
        return spawnPosition;

    }

    // Generates random square index from 0 to 3, which determines which square the target will appear in
    int RandomSquareIndex()
    {
        return Random.Range(0, 4);
    }

    // Update score with value from target clicked
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "score: " + score;
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
