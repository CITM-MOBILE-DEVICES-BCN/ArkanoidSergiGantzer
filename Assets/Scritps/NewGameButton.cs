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
    }

    void OnNewGameButtonPressed()
    {
        audioSource.Play();
        StartCoroutine(WaitForAudioToEnd());
    }

    IEnumerator WaitForAudioToEnd()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        mainMenu.PlayGame();
    }
}
