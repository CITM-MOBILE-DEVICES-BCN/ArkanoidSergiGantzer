using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Variables
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

    // Métodos
    private void Start()
    {
        // Inicialización de variables
        ballRb = GetComponent<Rigidbody2D>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.25f;
        // Inicialización de la velocidad inicial
        float randomX = Random.Range(-4f, 4f);
        initialVelocity = new Vector2(randomX, initialVelocity.y);
        initialVelocityMagnitude = initialVelocity.magnitude;
        originalScale = transform.localScale;
        // Posicionamiento de la bola sobre la plataforma
        PositionAbovePlatform();
        StartCoroutine(AutoLaunch());
    }
    // Métodos
    public IEnumerator AutoLaunch()
    {
        yield return new WaitForSeconds(2f);
        Launch();
    }
    // Métodos
    public void IncreaseSpeed()
    {
        // Aumento de la velocidad de la bola
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
    // Métodos
    public void PositionAbovePlatform()
    {
        // Posicionamiento de la bola sobre la plataforma
        if (transform.parent != null)
        {
            transform.position = transform.parent.position + new Vector3(0, 1f, 0);
        }
    }

    private void Update()
    {
        // Seguir la plataforma si no se ha lanzado
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
        // Seguir la plataforma si no se ha lanzado
        if (transform.parent != null)
        {
            transform.position = transform.parent.position + new Vector3(0, 1f, 0);
        }
    }
    // Métodos
    public void Launch()
    {
        // Lanzamiento de la bola
        isLaunched = true;
        ballRb.velocity = initialVelocity;
        transform.parent = null;
        PlaySound(launchSound);
    }
    // Métodos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reproducir sonido de colisión
        EnforceMinimumVelocity();
        PlaySound(collisionSound);
    }
    // Métodos
    private void EnforceMinimumVelocity()
    {
        // Ajustar la velocidad mínima de la bola
        if (ballRb.velocity.magnitude < initialVelocityMagnitude)
        {
            ballRb.velocity = ballRb.velocity.normalized * initialVelocityMagnitude;
        }
    }
    // Métodos
    private void VelocityFix()
    {
        // Ajustar la velocidad de la bola
        ballRb.velocity *= velocityMultiplier;
    }
    // Métodos
    public void PlaySound(AudioClip clip)
    {
        // Reproducir sonido
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    // Métodos
    public void ChangeBallSize(float scaleMultiplier)
    {
        // Cambiar el tamaño de la bola
        transform.localScale += new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);
    }

    private void AdjustDirectionIfStuck()
    {
        // Ajustar la dirección de la bola si se queda atascada
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

