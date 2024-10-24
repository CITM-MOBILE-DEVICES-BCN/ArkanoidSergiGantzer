using UnityEngine;

public class Buttons : MonoBehaviour
{
    // Variables
    public PauseMenu pauseMenu;
    public GameManager gameManager;

    public void OnContinueButtonPressed()
    {
        // Continuar la partida
        pauseMenu.Resume();
    }

    public void OnMainMenuButtonPressed()
    {
        // Cargar el menú principal
        pauseMenu.LoadMainMenu();
    }
}
