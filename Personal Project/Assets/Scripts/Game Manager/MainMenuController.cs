
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string gameplayScene = "Gameplay";
    [SerializeField] private GameObject continueButton;

    private JsonSaveRepository repository;
    private LocalProfileRepository profileRepository;

    private void Awake()
    {
        repository = new JsonSaveRepository();
        profileRepository = new LocalProfileRepository();
    }

    private void Start()
    {
        if (continueButton != null)
            continueButton.SetActive(repository.Exists);
    }

    public void NewGame()
    {
        GameSession.RequestNewGame();
        SceneManager.LoadScene(gameplayScene);
    }

    public void ContinueGame()
    {
        if (!repository.Exists)
            return;

        GameSession.RequestContinue();
        SceneManager.LoadScene(gameplayScene);
    }

    // public void PlayGame()
    // {
    //     Time.timeScale = 1f;
    //     SceneManager.LoadScene(gameplayScene);
    // }

    public void PlayGame()
    {
        if (!profileRepository.Exists)
        {
            // Display registration panel.
            return;
        }

        if (repository.Exists)
        {
            GameSession.RequestContinue();
        }
        else
        {
            GameSession.RequestNewGame();
        }

        SceneManager.LoadScene(gameplayScene);
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
