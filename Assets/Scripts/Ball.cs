using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float ballCollisionForce = 1f;

    [SerializeField]
    private float wallCollisionForce = 1f;

    [SerializeField]
    private TMP_Text devText;

    private float ballCollisionForceTimeOffset = 0.1f;
    private int ballTouchCounter = 0;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private InputAction respawnAction;
    private PlayerInput playerInput;
    private Rigidbody ballRigidbody;

    private void Awake()
    {
        InitializeVariables();
    }

    private void Start()
    {
        HandleInputs();
        RememberStartPosition();
    }

    private void InitializeVariables()
    {
        ballRigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        respawnAction = playerInput.actions["Respawn"];
    }

    private void HandleInputs()
    {
        respawnAction.performed += context => Respawn();
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
        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    private void RememberStartPosition()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void Respawn()
    {
        ballRigidbody.velocity = Vector3.zero; // Setze die Geschwindigkeit auf Null
        ballRigidbody.angularVelocity = Vector3.zero; // Setze den Drehimpuls auf Null
        SetPositionToStartPosition();
    }
}
