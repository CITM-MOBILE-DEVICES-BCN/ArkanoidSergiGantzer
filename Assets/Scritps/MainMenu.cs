using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Load the last played level
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        SceneManager.LoadSceneAsync(currentLevel -1);
    }

    public void NewGame()
    {
        // Reset the game state
        PlayerPrefs.SetInt("CurrentScore", 0);
        PlayerPrefs.SetInt("CurrentLives", 3);
        PlayerPrefs.SetInt("CurrentLevel", 1);
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
