
using System;
using TMPro;
using UnityEngine;

public class PlayerRegistrationUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject registrationPanel;
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Registration")]
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_Text statusText;

    [Header("Main Menu")]
    [SerializeField] private TMP_Text welcomeText;

    private LocalProfileRepository repository;

    private void Awake()
    {
        repository = new LocalProfileRepository();
    }

    private void Start()
    {
        PlayerProfile profile = repository.Load();

        if (profile == null)
        {
            ShowRegistration();
        }
        else
        {
            ShowMainMenu(profile);
        }
    }

    private void ShowRegistration()
    {
        if (registrationPanel != null)
            registrationPanel.SetActive(true);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
    }

    private void ShowMainMenu(PlayerProfile profile)
    {
        if (welcomeText != null)
        {
            welcomeText.text =
                $"Welcome, {profile.username}!";
        }

        if (registrationPanel != null)
            registrationPanel.SetActive(false);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    public void RegisterPlayer()
    {
        try
        {
            PlayerProfile profile =
                repository.Register(usernameInput.text);

            ShowMainMenu(profile);
        }
        catch (Exception exception)
        {
            if (statusText != null)
                statusText.text = exception.Message;
        }
    }
}



// using System;
// using TMPro;
// using UnityEngine;

// public class PlayerRegistrationUI : MonoBehaviour
// {
//     [SerializeField] private GameObject registrationPanel;
//     [SerializeField] private TMP_InputField usernameInput;
//     [SerializeField] private TMP_Text statusText;
//     [SerializeField] private TMP_Text welcomeText;

//     private LocalProfileRepository repository;

//     private void Awake()
//     {
//         repository = new LocalProfileRepository();
//     }

//     private void Start()
//     {
//         PlayerProfile profile = repository.Load();

//         bool needsRegistration = profile == null;

//         registrationPanel.SetActive(needsRegistration);

//         if (!needsRegistration && welcomeText != null)
//             welcomeText.text = $"Welcome, {profile.username}!";
//     }

//     public void RegisterPlayer()
//     {
//         try
//         {
//             PlayerProfile profile =
//                 repository.Register(usernameInput.text);

//             if (welcomeText != null)
//                 welcomeText.text = $"Welcome, {profile.username}!";

//             registrationPanel.SetActive(false);
//         }
//         catch (Exception exception)
//         {
//             if (statusText != null)
//                 statusText.text = exception.Message;
//         }
//     }
// }
