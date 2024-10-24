using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class LevelClearManager : MonoBehaviour
{
    // Variables
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Inicializaci�n de variables
        videoPlayer.Play();
        StartCoroutine(WaitAndLoadNextLevel(5f));
    }
    // M�todos
    IEnumerator WaitAndLoadNextLevel(float waitTime)
    {
        // Espera a que termine la reproducci�n del v�deo
        yield return new WaitForSeconds(waitTime);
        OnVideoFinished(videoPlayer);
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Carga del siguiente nivel
        GameManager.Instance.LoadNextLevel();
    }

    void OnDestroy()
    {
        // Liberaci�n de recursos
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}
