using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro; // MIGRATED: New Input System namespace

public class MainManager : MonoBehaviour
{
    // public static MainManager Instance {get; private set;}
    
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    [SerializeField] private TextMeshProUGUI ScoreAndNameText;
    [SerializeField] private TextMeshProUGUI globalHighScoreText;
    [SerializeField] private TextMeshProUGUI personalHighScoreText;
    [SerializeField] private GameObject GameOverText;
    [SerializeField] private GameObject backToMenu;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    // MIGRATED: InputAction replaces Input.GetKeyDown(KeyCode.Space)
    private InputAction m_LaunchAction;

    // MIGRATED: bind the Space key as a button action
    void Awake()
    {
        m_LaunchAction = new InputAction("Launch", InputActionType.Button, "<Keyboard>/space");
    }

    // MIGRATED: enable the action while the component is active
    void OnEnable()
    {
        m_LaunchAction.Enable();
    }

    // MIGRATED: disable the action when the component is inactive
    void OnDisable()
    {
        m_LaunchAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        backToMenu.SetActive(false);

        UpdateUIDisplays();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (m_LaunchAction.WasPressedThisFrame()) // MIGRATED: was Input.GetKeyDown(KeyCode.Space)
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (m_LaunchAction.WasPressedThisFrame()) // MIGRATED: was Input.GetKeyDown(KeyCode.Space)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;

        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.UpdateScores(m_Points);
        }
        UpdateUIDisplays();
        // ScoreText.text = $"Score : {m_Points}";
    }

    private void UpdateUIDisplays()
    {
        // string playerName = PlayerData.Instance != null ? PlayerData.Instance.playerName : "Player";

        ScoreAndNameText.text = $"{PlayerData.Instance.playerName} | Score : {m_Points}";
        
        int personalBest = PlayerData.Instance.GetPersonalBest(PlayerData.Instance.playerName);
        if (personalHighScoreText != null)
        {
            personalHighScoreText.text = $"Your Best: {personalBest}";
        }

                // Community master record score lookup
        if (globalHighScoreText != null)
        {            
            globalHighScoreText.text = $"All-Time Best: {PlayerData.Instance.globalHighScoreName} ({PlayerData.Instance.globalHighScore})";
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        backToMenu.SetActive(true);
    }
}
