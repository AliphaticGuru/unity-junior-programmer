using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TextMeshProUGUI overallBestText;
    [SerializeField] private TextMeshProUGUI personalBestText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisplayGlobalRecord();
        // LoadAndDisplayBestScore();
        if (nameInputField != null)
        {
            nameInputField.onValueChanged.AddListener(OnNameInputChanged);
            OnNameInputChanged(nameInputField.text);
        }
    }
    void OnDestroy()
    {
        if (nameInputField != null)
        {
            nameInputField.onValueChanged.RemoveListener(OnNameInputChanged);
        }
    }

    private void DisplayGlobalRecord()
    {
        if (overallBestText != null && PlayerData.Instance != null)
        {
            int globalScore = PlayerData.Instance.globalHighScore;
            string globalHolder = PlayerData.Instance.globalHighScoreName;

            if (globalScore > 0)
                overallBestText.text = $"All-Time Record: {globalHolder} ({globalScore})";
            else
                overallBestText.text = "No All-Time Record Yet!";
        }
    }

    private void OnNameInputChanged(string enteredName)
    {
        if (personalBestText == null || PlayerData.Instance == null) return;

        if (string.IsNullOrWhiteSpace(enteredName))
        {
            personalBestText.text = "Enter a name to load profile...";
            return;
        }

        int personalBest = PlayerData.Instance.GetPersonalBest(enteredName);

        if (personalBest > 0)
            personalBestText.text = $"{enteredName.Trim()}'s Personal Best: {personalBest}";
        else
            personalBestText.text = $"Welcome {enteredName.Trim()}! (New Profile)";
    }

    public void StartNew()
    {
        if (nameInputField == null || PlayerData.Instance == null) return;

        string chosenName = nameInputField.text.Trim();
        if (string.IsNullOrWhiteSpace(chosenName)) chosenName = "Player";

        // Assign the profile selection to our persistent Singleton instance
        PlayerData.Instance.playerName = chosenName;
        PlayerData.Instance.userScore = 0; // Reset active session points counter

        SceneManager.LoadScene(1);
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("BestScore");
        PlayerPrefs.DeleteKey("HighScoreName");
        PlayerPrefs.Save();

        DisplayGlobalRecord();
    }
    
    public void Exit()
    { 
        #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
        #else
                Application.Quit();
        #endif
    }
}
