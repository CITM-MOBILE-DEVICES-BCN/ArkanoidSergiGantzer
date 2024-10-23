using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        SaveGame();
        Time.timeScale = 1f;
        SceneManager.LoadScene(3);
    }

    public void HandleButtonAction(string action)
    {
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
        PlayerPrefs.SetInt("CurrentScore", GameManager.Instance.GetScore());
        PlayerPrefs.SetInt("CurrentLives", GameManager.Instance.GetLives());
        PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel", 1));
    }
}
