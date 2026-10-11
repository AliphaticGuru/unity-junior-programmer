
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePauseInput : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed || gameManager == null)
            return;

        switch (gameManager.CurrentState)
        {
            case GameState.Playing:
                gameManager.PauseGame();
                break;

            case GameState.Paused:
                gameManager.ResumeGame();
                break;
        }
    }
}
