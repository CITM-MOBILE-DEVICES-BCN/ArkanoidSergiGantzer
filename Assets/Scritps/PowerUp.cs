using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float fallSpeed = 2f;
    public AudioClip powerUpSound;

    private void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (powerUpSound != null)
            {
                AudioSource.PlayClipAtPoint(powerUpSound, transform.position);
            }
            Platform platformScript = collision.gameObject.GetComponent<Platform>();
            if (platformScript != null)
            {
                platformScript.IncreaseBallSize();
            }

            Destroy(gameObject);
        }
    }
}
