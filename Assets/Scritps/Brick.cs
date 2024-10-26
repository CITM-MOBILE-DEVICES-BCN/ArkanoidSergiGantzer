using UnityEngine;

public class Brick : MonoBehaviour
{
    // Variables
    public int hp;
    private SpriteRenderer spriteRenderer;
    private Collider2D blockCollider;
    public Sprite[] blockSprites;
    private bool isColliding = false;
    public AudioClip destroySound;
    private AudioSource audioSource;

    public GameObject powerUpPrefab;
    private float powerUpDropChance = 0.1f;

    private void Start()
    {
        // Inicialización de variables
        spriteRenderer = GetComponent<SpriteRenderer>();
        blockCollider = GetComponent<Collider2D>();
        audioSource = gameObject.AddComponent<AudioSource>();
        UpdateBlockAppearance();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Colisión con la bola
        if (isColliding) return;

        if (collision.gameObject.CompareTag("Ball"))
        {
            // Actualización de la vida del bloque
            isColliding = true;
            hp--;
            if (hp > 0)
            {
                UpdateBlockAppearance();
                GameManager.Instance.BlockHit();
            }
            else
            {
                // Destrucción del bloque
                PlayDestroySound();
                TryDropPowerUp();
                Destroy(gameObject);
                GameManager.Instance.BlockDestroyed();

                // Increase ball speed when a block is destroyed
                Ball ballScript = collision.gameObject.GetComponent<Ball>();
                if (ballScript != null)
                {
                    ballScript.IncreaseSpeed();
                }
            }
            Invoke("ResetCollisionFlag", 0.1f);
        }
    }

    private void UpdateBlockAppearance()
    {
        // Actualización del sprite del bloque
        if (hp > 0 && hp <= blockSprites.Length)
        {
            spriteRenderer.sprite = blockSprites[blockSprites.Length - hp];
        }
    }

    private void ResetCollisionFlag()
    {
        // Reinicio de la bandera de colisión
        isColliding = false;
    }

    private void PlayDestroySound()
    {
        // Reproducción del sonido de destrucción
        if (destroySound != null)
        {
            GameObject soundObject = new GameObject("BlockDestroySound");
            AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();

            tempAudioSource.clip = destroySound;
            tempAudioSource.Play();

            Destroy(soundObject, destroySound.length);
        }
    }

    private void TryDropPowerUp()
    {
        // Probabilidad de soltar power-up
        if (powerUpPrefab != null && Random.value <= powerUpDropChance)
        {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }
    }
}
