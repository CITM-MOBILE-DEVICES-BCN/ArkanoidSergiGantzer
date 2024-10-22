using UnityEngine;

public class Buttons : MonoBehaviour
{
    public PauseMenu pauseMenu;
    public GameManager gameManager;

    public void OnContinueButtonPressed()
    {
        pauseMenu.Resume();
    }

    public void OnMainMenuButtonPressed()
    {
        pauseMenu.LoadMainMenu();
    }
}
