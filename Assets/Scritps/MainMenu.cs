using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Cargar la partida guardada
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        SceneManager.LoadSceneAsync(currentLevel -1);
    }

    public void NewGame()
    {
        // Iniciar una nueva partida
        PlayerPrefs.SetInt("CurrentScore", 0);
        PlayerPrefs.SetInt("CurrentLives", 3);
        PlayerPrefs.SetInt("CurrentLevel", 1);
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        // Salir del juego
        Application.Quit();
    }
}
