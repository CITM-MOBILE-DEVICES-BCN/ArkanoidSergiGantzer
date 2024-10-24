using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Variables
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
        // Singleton pattern
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
        // Inicializaci�n de variables
        blocksLeft = GameObject.FindGameObjectsWithTag("Block").Length;
        audioSource = gameObject.AddComponent<AudioSource>();
        score = PlayerPrefs.GetInt("CurrentScore", 0);
        lives = PlayerPrefs.GetInt("CurrentLives", 3);
        UpdateUI();
        RespawnBall();
    }

    public void BlockDestroyed()
    {
        // Actualizaci�n de la puntuaci�n y los bloques restantes
        IncreaseScore(50);
        blocksLeft--;
        UpdateUI();

        if (blocksLeft <= 0 && !isLevelCleared)
        {
            // Comprobaci�n de nivel completado 
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
        // Actualizaci�n de la puntuaci�n
        IncreaseScore(10);
        UpdateUI();
    }

    public void LoadNextLevel()
    {
        // Carga de la siguiente escena
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
        // Recarga de la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BallFell()
    {
        // Actualizaci�n de vidas
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
        // Carga de la escena de fin de partida
        SceneManager.LoadScene(4);
    }

    public void RespawnBall()
    {
        // Respawn de la bola
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
        // Actualizaci�n de la interfaz de usuario
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
        levelText.text = "Level: " + PlayerPrefs.GetInt("CurrentLevel", 1);
    }

    private void PlaySound(AudioClip clip)
    {
        // Reproducci�n de sonido
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void IncreaseScore(int amount)
    {
        // Aumento de la puntuaci�n
        score += amount;
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    public int GetScore()
    {
        // Obtenci�n de la puntuaci�n
        return score;
    }

    public int GetLives()
    {
        // Obtenci�n de las vidas
        return lives;
    }

    public void ResetGameState()
    {
        // Reinicio del estado del juego
        Debug.Log("GameManager: ResetGameState method called.");
        score = 0;
        lives = 3;
        PlayerPrefs.SetInt("CurrentLevel", 1);
        PlayerPrefs.SetInt("CurrentScore", score);
        PlayerPrefs.SetInt("CurrentLives", lives);
        UpdateUI();
        SceneManager.LoadScene(0);
    }
}
