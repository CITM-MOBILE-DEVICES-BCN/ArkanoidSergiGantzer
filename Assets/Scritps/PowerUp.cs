using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Variables
    public float fallSpeed = 2f;
    public AudioClip powerUpSound;

    private void Update()
    {
        // Movimiento del power-up
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Colisión con la plataforma
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (powerUpSound != null)
            {
                AudioSource.PlayClipAtPoint(powerUpSound, transform.position);
            }
            // Aumento del tamaño de la bola
            Platform platformScript = collision.gameObject.GetComponent<Platform>();
            if (platformScript != null)
            {
                platformScript.IncreaseBallSize();
            }

            Destroy(gameObject);
        }
    }
}
