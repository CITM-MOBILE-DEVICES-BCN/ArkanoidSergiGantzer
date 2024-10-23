using UnityEngine;
using UnityEngine.UI;

public class Platform : MonoBehaviour
{
    [SerializeField] private Slider movementSlider;
    [SerializeField] private float smoothTime = 0.05f;

    private float velocity = 0.0f;
    private GameObject ball;
    private bool isAutoMode = false;
    private float initialScreenWidth;

    private Rigidbody2D rb;

    // New public GameObjects for bounds
    public GameObject leftBound;
    public GameObject rightBound;

    private void Start()
    {
        FindBall();
        movementSlider.value = 0.5f;
        initialScreenWidth = Screen.width;
        rb = GetComponent<Rigidbody2D>();
    }

    public void FindBall()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isAutoMode = !isAutoMode;
        }

        // Check for screen size change
        if (Screen.width != initialScreenWidth)
        {
            AdjustSlider();
            initialScreenWidth = Screen.width;
        }

        Vector2 playerPosition = rb.position;

        if (isAutoMode)
        {
            GameObject target = FindClosestTarget();
            if (target != null)
            {
                float targetPositionX = target.transform.position.x;
                playerPosition.x = targetPositionX; // Instant movement without smoothing
            }
        }
        else
        {
            float targetPositionX = Mathf.Lerp(-7.5f, 7.5f, movementSlider.value);
            playerPosition.x = Mathf.SmoothDamp(playerPosition.x, targetPositionX, ref velocity, smoothTime);
        }

        // Check for collisions with bounds
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
        // Adjust slider value proportionally
        float newSliderValue = movementSlider.value * (initialScreenWidth / Screen.width);
        movementSlider.value = Mathf.Clamp(newSliderValue, 0, 1);
    }

    private GameObject FindClosestTarget()
    {
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
        if (other.CompareTag("PowerUp"))
        {
            GameManager.Instance.IncreaseScore(100);
            Destroy(other.gameObject);
        }
    }
}
