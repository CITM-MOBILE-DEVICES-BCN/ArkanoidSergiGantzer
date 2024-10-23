using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour
{
    public Button newGameButton;
    public AudioClip audioClip;
    public MainMenu mainMenu;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;

        newGameButton.onClick.AddListener(OnNewGameButtonPressed);
        Debug.Log("NewGameButton: Start method called, listener added.");
    }

    void OnNewGameButtonPressed()
    {
        Debug.Log("NewGameButton: OnNewGameButtonPressed method called.");
        audioSource.Play();
        StartCoroutine(WaitForAudioToEnd());
    }

    IEnumerator WaitForAudioToEnd()
    {
        Debug.Log("NewGameButton: WaitForAudioToEnd coroutine started.");
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        Debug.Log("NewGameButton: Audio finished playing, calling ResetGame.");
        ResetGame();
        mainMenu.PlayGame();
    }

    private void ResetGame()
    {
        Debug.Log("NewGameButton: ResetGame method called.");
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("CurrentLevel", 1);
        PlayerPrefs.SetInt("CurrentScore", 0);
        PlayerPrefs.SetInt("CurrentLives", 3);
        GameManager.Instance.ResetGameState();
    }
}
