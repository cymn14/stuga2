using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float ballCollisionForce = 1f;

    [SerializeField]
    private float wallCollisionForce = 1f;

    [SerializeField]
    private TMP_Text devText;

    [SerializeField]
    private Transform startPosition;

    private float ballCollisionForceTimeOffset = 0.1f;
    private int ballTouchCounter = 0;

    private void Start()
    {
        SetPositionToStartPosition();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Goal")
        {
            print("You win");
        }

        else if (other.tag == "Player")
        {
            print("player trigger");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            StartCoroutine(AddBallForce(ballCollisionForceTimeOffset, ballCollisionForce));
            BallTouched();
        }

        if (collision.collider.tag == "Wall")
        {
            StartCoroutine(AddBallForce(ballCollisionForceTimeOffset, wallCollisionForce));
        }
    }

    IEnumerator AddBallForce(float time, float ballForce)
    {
        yield return new WaitForSeconds(time);
        Rigidbody ballRb = gameObject.GetComponent<Rigidbody>();
        ballRb.AddForce(ballRb.velocity * ballForce, ForceMode.Impulse);
    }
    
    private void BallTouched()
    {
        ballTouchCounter++;
        //devText.text = ballTouchCounter.ToString();
    }

    private void SetPositionToStartPosition()
    {
        transform.position = startPosition.position;
        transform.rotation = startPosition.rotation;
    }
}
