using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Vector2 initialVelocity;
    [SerializeField] private float velocityMultiplier;
    [SerializeField] private AudioClip collisionSound;
    [SerializeField] private AudioClip launchSound;
    private Rigidbody2D ballRb;
    private AudioSource audioSource;
    private float initialVelocityMagnitude;
    private Vector3 originalScale;
    public bool isLaunched;
    private int speedIncreaseCount = 0;
    private const int maxSpeedIncreases = 20;

    private void Start()
    {
        ballRb = GetComponent<Rigidbody2D>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.25f;

        float randomX = Random.Range(-4f, 4f);
        initialVelocity = new Vector2(randomX, initialVelocity.y);
        initialVelocityMagnitude = initialVelocity.magnitude;
        originalScale = transform.localScale;

        PositionAbovePlatform();
        StartCoroutine(AutoLaunch());
    }

    public IEnumerator AutoLaunch()
    {
        yield return new WaitForSeconds(2f);
        Launch();
    }

    public void IncreaseSpeed()
    {
        if (speedIncreaseCount < maxSpeedIncreases)
        {
            ballRb.velocity *= 1.05f;
            speedIncreaseCount++;
        }
        else
        {
            // Mantener la velocidad constante una vez alcanzado el límite
            ballRb.velocity = ballRb.velocity.normalized * initialVelocityMagnitude * Mathf.Pow(1.05f, maxSpeedIncreases);
        }
    }

    public void PositionAbovePlatform()
    {
        if (transform.parent != null)
        {
            transform.position = transform.parent.position + new Vector3(0, 1f, 0);
        }
    }

    private void Update()
    {
        if (!isLaunched)
        {
            FollowPlatform();
        }
        else
        {
            AdjustDirectionIfStuck();
        }
    }

    private void FollowPlatform()
    {
        if (transform.parent != null)
        {
            transform.position = transform.parent.position + new Vector3(0, 1f, 0);
        }
    }

    public void Launch()
    {
        isLaunched = true;
        ballRb.velocity = initialVelocity;
        transform.parent = null;
        PlaySound(launchSound);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnforceMinimumVelocity();
        PlaySound(collisionSound);
    }

    private void EnforceMinimumVelocity()
    {
        if (ballRb.velocity.magnitude < initialVelocityMagnitude)
        {
            ballRb.velocity = ballRb.velocity.normalized * initialVelocityMagnitude;
        }
    }

    private void VelocityFix()
    {
        ballRb.velocity *= velocityMultiplier;
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void ChangeBallSize(float scaleMultiplier)
    {
        transform.localScale += new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);
    }

    private void AdjustDirectionIfStuck()
    {
        if (Mathf.Abs(ballRb.velocity.x) < 0.1f)
        {
            ballRb.velocity = new Vector2(ballRb.velocity.x + Random.Range(-0.5f, 0.5f), ballRb.velocity.y);
        }
        if (Mathf.Abs(ballRb.velocity.y) < 0.1f)
        {
            ballRb.velocity = new Vector2(ballRb.velocity.x, ballRb.velocity.y + Random.Range(-0.5f, 0.5f));
        }
    }
}

