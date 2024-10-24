using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class LevelClearManager : MonoBehaviour
{
    // Variables
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Inicialización de variables
        videoPlayer.Play();
        StartCoroutine(WaitAndLoadNextLevel(5f));
    }
    // Métodos
    IEnumerator WaitAndLoadNextLevel(float waitTime)
    {
        // Espera a que termine la reproducción del vídeo
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
        // Liberación de recursos
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}
