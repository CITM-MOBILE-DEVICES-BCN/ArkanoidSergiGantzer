using UnityEngine;
using System.Collections;

public class LoseTrigger : MonoBehaviour
{
    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered && collision.CompareTag("Ball"))
        {
            isTriggered = true;
            Ball ball = collision.GetComponent<Ball>();
            if (ball != null)
            {
                StartCoroutine(ResetAndRelaunchBall(ball));
            }
            Destroy(collision.gameObject);
            GameManager.Instance.BallFell();
            StartCoroutine(ResetTrigger());
        }
    }

    private IEnumerator ResetAndRelaunchBall(Ball ball)
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.RespawnBall();
    }

    private IEnumerator ResetTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        isTriggered = false;
    }
}
