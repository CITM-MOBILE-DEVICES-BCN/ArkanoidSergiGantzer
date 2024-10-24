using UnityEngine;
using System.Collections;

public class LoseTrigger : MonoBehaviour
{
    private bool isTriggered = false;
    // Métodos
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Colisión con la bola
        if (!isTriggered && collision.CompareTag("Ball"))
        {
            isTriggered = true;
            Ball ball = collision.GetComponent<Ball>();
            if (ball != null)
            {
                StartCoroutine(ResetAndRelaunchBall(ball));
            }
            // Destrucción de la bola
            Destroy(collision.gameObject);
            GameManager.Instance.BallFell();
            StartCoroutine(ResetTrigger());
        }
    }
    // Métodos
    private IEnumerator ResetAndRelaunchBall(Ball ball)
    {
        // Respawn de la bola
        yield return new WaitForSeconds(2f);
        GameManager.Instance.RespawnBall();
    }

    private IEnumerator ResetTrigger()
    {
        // Reset del trigger
        yield return new WaitForSeconds(0.5f);
        isTriggered = false;
    }
}
