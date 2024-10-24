using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour
{
    // Variables
    public Button newGameButton;
    public AudioClip audioClip;
    public MainMenu mainMenu;
    private AudioSource audioSource;

    void Start()
    {
        // Inicialización de variables
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;

        newGameButton.onClick.AddListener(OnNewGameButtonPressed);
    }

    void OnNewGameButtonPressed()
    {
        // Inicio de una nueva partida
        audioSource.Play();
        StartCoroutine(WaitForAudioToEnd());
    }

    IEnumerator WaitForAudioToEnd()
    {
        // Espera a que termine la reproducción del audio
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        ResetGame();
        mainMenu.PlayGame();
    }

    private void ResetGame()
    {
        // Reinicio de la partida
        Debug.Log("NewGameButton: ResetGame method called.");
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("CurrentLevel", 1);
        PlayerPrefs.SetInt("CurrentScore", 0);
        PlayerPrefs.SetInt("CurrentLives", 3);
        GameManager.Instance.ResetGameState();
        SceneManager.LoadScene(0);
    }
}
