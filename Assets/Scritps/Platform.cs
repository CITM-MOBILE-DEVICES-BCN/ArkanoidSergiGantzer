using UnityEngine;
using UnityEngine.UI;

public class Platform : MonoBehaviour
{
    [SerializeField] private Slider movementSlider;
    [SerializeField] private float bounds = 7.5f;
    [SerializeField] private float smoothTime = 0.05f;

    private float velocity = 0.0f;
    private GameObject ball;
    private bool isAutoMode = false;

    private void Start()
    {
        FindBall();
        movementSlider.value = 0.5f;
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

        Vector2 playerPosition = transform.position;

        if (isAutoMode)
        {
            GameObject target = FindClosestTarget();
            if (target != null)
            {
                float targetPositionX = Mathf.Clamp(target.transform.position.x, -bounds, bounds);
                playerPosition.x = targetPositionX; // Movimiento instantáneo sin suavizado
            }
        }
        else
        {
            float targetPositionX = Mathf.Lerp(-bounds, bounds, movementSlider.value);
            playerPosition.x = Mathf.SmoothDamp(playerPosition.x, targetPositionX, ref velocity, smoothTime);
        }

        transform.position = playerPosition;
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
