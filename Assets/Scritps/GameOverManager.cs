using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Inicializaci�n de variables
        videoPlayer.Play();

        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Carga de la pantalla de Game Over cuando acaba el v�deo
        SceneManager.LoadSceneAsync(3);
    }

    void OnDestroy()
    {
        // Liberaci�n de recursos
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}
