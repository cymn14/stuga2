using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float ballCollisionForce = 1f;

    [SerializeField]
    private float wallCollisionForce = 1f;

    [SerializeField]
    private bool gravityDisabled = false;

    private float ballCollisionForceTimeOffset = 0.1f;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private InputAction respawnAction;
    private PlayerInput playerInput;
    private Rigidbody ballRigidbody;

    private void Awake()
    {
        InitializeVariables();

        if (gravityDisabled)
        {
            ballRigidbody.useGravity = false;
        }
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
        respawnAction.performed += context => Reset();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            if (gravityDisabled)
            {
                ballRigidbody.useGravity = true;
            }

            StartCoroutine(AddBallForce(ballCollisionForceTimeOffset, ballCollisionForce));
        }

        if (collision.collider.tag == "Wall")
        {
            StartCoroutine(AddBallForce(ballCollisionForceTimeOffset, wallCollisionForce));
        }
    }

    IEnumerator AddBallForce(float time, float ballForce)
    {
        yield return new WaitForSeconds(time);
        ballRigidbody.AddForce(ballRigidbody.velocity * ballForce, ForceMode.Impulse);
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

    public void Reset()
    {
        if (gravityDisabled)
        {
            ballRigidbody.useGravity = false;
        }

        gameObject.SetActive(false);
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        gameObject.SetActive(true);
        SetPositionToStartPosition();
    }
}
