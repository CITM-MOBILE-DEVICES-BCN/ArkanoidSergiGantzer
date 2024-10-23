using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] public GameObject ballPrefab;
    [SerializeField] private Transform platform;
    [SerializeField] private AudioClip lifeLostSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private Platform platformObject;

    private int blocksLeft;
    private int score = 0;
    private int lives = 3;
    private AudioSource audioSource;
    private bool isLevelCleared = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        blocksLeft = GameObject.FindGameObjectsWithTag("Block").Length;
        audioSource = gameObject.AddComponent<AudioSource>();
        score = PlayerPrefs.GetInt("CurrentScore", 0);
        lives = PlayerPrefs.GetInt("CurrentLives", 3);
        UpdateUI();
        RespawnBall();
    }

    public void BlockDestroyed()
    {
        IncreaseScore(50);
        blocksLeft--;
        UpdateUI();

        if (blocksLeft <= 0 && !isLevelCleared)
        {
            isLevelCleared = true;
            int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
            PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
            PlayerPrefs.SetInt("CurrentScore", score);
            PlayerPrefs.SetInt("CurrentLives", lives);
            LoadNextLevel();
        }
    }

    public void BlockHit()
    {
        IncreaseScore(10);
        UpdateUI();
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 5)
        {
            currentSceneIndex = PlayerPrefs.GetInt("LastLevelIndex", 0);
            if (currentSceneIndex == 2)
            {
                SceneManager.LoadScene(0); // Vuelve al nivel 1 (escena 0)
            }
            else
            {
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
        }
        else
        {
            PlayerPrefs.SetInt("LastLevelIndex", currentSceneIndex);
            SceneManager.LoadScene(5); // Carga la escena de nivel completado
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BallFell()
    {
        lives--;
        if (lives != 0)
        {
            PlaySound(lifeLostSound);
        }

        if (lives <= 0)
        {
            PlaySound(gameOverSound);
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            if (score > highScore)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
            Invoke("LoadGameOverScene", gameOverSound.length);
        }

        UpdateUI();
    }

    private void LoadGameOverScene()
    {
        SceneManager.LoadScene(4);
    }

    public void RespawnBall()
    {
        GameObject newBall = Instantiate(ballPrefab, platform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        newBall.transform.SetParent(platform, false);
        newBall.transform.localScale = new Vector3(1f, 1f, 1f);
        Ball newBallScript = newBall.GetComponent<Ball>();
        platformObject.FindBall();
        newBallScript.isLaunched = false;
        newBallScript.PositionAbovePlatform();
        newBallScript.StartCoroutine(newBallScript.AutoLaunch());
    }

    private void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
        levelText.text = "Level: " + PlayerPrefs.GetInt("CurrentLevel", 1);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public int GetLives()
    {
        return lives;
    }

    public void ResetGameState()
    {
        Debug.Log("GameManager: ResetGameState method called.");
        score = 0;
        lives = 3;
        PlayerPrefs.SetInt("CurrentLevel", 1);
        PlayerPrefs.SetInt("CurrentScore", score);
        PlayerPrefs.SetInt("CurrentLives", lives);
        UpdateUI();
    }
}
