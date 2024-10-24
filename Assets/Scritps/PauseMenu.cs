using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Variables
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Start()
    {
        // Inicialización de variables
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        // Pausa del juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        //  Continuar la partida
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        // Pausar la partida
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        // Cargar el menú principal
        SaveGame();
        Time.timeScale = 1f;
        SceneManager.LoadScene(3);
    }

    public void HandleButtonAction(string action)
    {
        // Manejo de las acciones de los botones
        switch (action)
        {
            case "Resume":
                Resume();
                break;
            case "LoadMainMenu":
                LoadMainMenu();
                break;

            default:
                break;
        }
    }

    private void SaveGame()
    {
        // Guardar la partida
        PlayerPrefs.SetInt("CurrentScore", GameManager.Instance.GetScore());
        PlayerPrefs.SetInt("CurrentLives", GameManager.Instance.GetLives());
        PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel", 1));
    }
}
