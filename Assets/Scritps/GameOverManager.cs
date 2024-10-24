using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Inicialización de variables
        videoPlayer.Play();

        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Carga de la pantalla de Game Over cuando acaba el vídeo
        SceneManager.LoadSceneAsync(3);
    }

    void OnDestroy()
    {
        // Liberación de recursos
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}
