using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class LevelClearManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.Play();
        StartCoroutine(WaitAndLoadNextLevel(5f));
    }

    IEnumerator WaitAndLoadNextLevel(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        OnVideoFinished(videoPlayer);
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        GameManager.Instance.LoadNextLevel();
    }

    void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}
