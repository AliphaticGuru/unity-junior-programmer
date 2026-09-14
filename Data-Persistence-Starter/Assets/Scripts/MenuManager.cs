using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    [SerializeField] private TMP_InputField playerNameInputField;


    // public void Awake()
    // {
    //     DontDestroyOnLoad(gameObject);
    // }
    public void StartNew()
    {
        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.playerName = playerNameInputField.text;
        }
        else
        {
            PlayerData.Instance.playerName = "Player";
        }

        // PlayerPrefs.SetString("PlayerName", PlayerData.Instance.playerName);
        // PlayerPrefs.Save();

        SceneManager.LoadScene(1);
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
