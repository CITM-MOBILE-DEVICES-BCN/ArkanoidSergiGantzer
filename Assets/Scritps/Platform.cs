using UnityEngine;
using UnityEngine.UI;

public class Platform : MonoBehaviour
{
    // Variables
    [SerializeField] private Slider movementSlider;
    [SerializeField] private float smoothTime = 0.05f;

    private float velocity = 0.0f;
    private GameObject ball;
    private bool isAutoMode = false;
    private float initialScreenWidth;

    private Rigidbody2D rb;

    // Métodos
    public GameObject leftBound;
    public GameObject rightBound;

    private void Start()
    {
        // Inicialización de variables
        FindBall();
        movementSlider.value = 0.5f;
        initialScreenWidth = Screen.width;
        rb = GetComponent<Rigidbody2D>();
    }

    public void FindBall()
    {
        // Buscar la bola en la escena
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    private void Update()
    {
        // Cambio de modo de control
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isAutoMode = !isAutoMode;
        }

        // Ajuste del slider
        if (Screen.width != initialScreenWidth)
        {
            AdjustSlider();
            initialScreenWidth = Screen.width;
        }

        Vector2 playerPosition = rb.position;

        if (isAutoMode)
        {
            // Movimiento automático
            GameObject target = FindClosestTarget();
            if (target != null)
            {
                float targetPositionX = target.transform.position.x;
                playerPosition.x = targetPositionX; // Instant movement without smoothing
            }
        }
        else
        {
            // Movimiento manual
            float targetPositionX = Mathf.Lerp(-7.5f, 7.5f, movementSlider.value);
            playerPosition.x = Mathf.SmoothDamp(playerPosition.x, targetPositionX, ref velocity, smoothTime);
        }

        // Limitar el movimiento a los límites
        if (leftBound != null && rightBound != null)
        {
            if (playerPosition.x < leftBound.transform.position.x)
            {
                playerPosition.x = leftBound.transform.position.x;
            }
            else if (playerPosition.x > rightBound.transform.position.x)
            {
                playerPosition.x = rightBound.transform.position.x;
            }
        }

        rb.MovePosition(playerPosition);
    }

    private void AdjustSlider()
    {
        // Ajuste del slider al cambiar el tamaño de la pantalla
        float newSliderValue = movementSlider.value * (initialScreenWidth / Screen.width);
        movementSlider.value = Mathf.Clamp(newSliderValue, 0, 1);
    }

    private GameObject FindClosestTarget()
    {
        // Buscar el objetivo más cercano
        GameObject closestTarget = ball;
        float closestDistance = Mathf.Infinity;

        GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (GameObject powerUp in powerUps)
        {
            float distance = Mathf.Abs(transform.position.y - powerUp.transform.position.y);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = powerUp;
            }
        }

        if (ball != null)
        {
            // Calcular la distancia a la bola
            float ballDistance = Mathf.Abs(transform.position.y - ball.transform.position.y);
            if (ballDistance < closestDistance || (ballDistance == closestDistance && closestTarget != ball))
            {
                closestTarget = ball;
            }
        }

        return closestTarget;
    }

    public void IncreaseBallSize()
    {
        // Aumentar el tamaño de la bola
        if (ball != null)
        {
            Ball ballScript = ball.GetComponent<Ball>();
            if (ballScript != null)
            {
                ballScript.ChangeBallSize(1.0f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Colisión con power-up
        if (other.CompareTag("PowerUp"))
        {
            GameManager.Instance.IncreaseScore(100);
            Destroy(other.gameObject);
        }
    }
}
