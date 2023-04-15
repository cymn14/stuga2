using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float moveSmoothness;

    [SerializeField]
    private float rotSmoothness;

    [SerializeField]
    private Vector3 moveOffset;

    [SerializeField]
    private Vector3 rotOffset;

    private Transform carTransform;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 targetPos;

    //private InputAction lookAroundAction;
    //private PlayerInput playerInput;

    //private Vector2 lookAroundVector; // Input des horizontalen Joystick-Achse

    //private float horizontalInput = 0f; // Input des horizontalen Joystick-Achse
    //private float verticalInput = 0f; // Input des vertikalen Joystick-Achse

    private void Awake()
    {
        InitializeVariables();
    }

    private void Start()
    {
        HandleInputs();
        SetPositionToCarPosition();
    }

    public void Reset()
    {
        SetPositionToCarPosition();
    }

    private void InitializeVariables()
    {
        carTransform = GameObject.Find("Car").GetComponent<Transform>();
        targetPos = carTransform.TransformPoint(moveOffset);

        //playerInput = GetComponent<PlayerInput>();
        //lookAroundAction = playerInput.actions["LookAround"];
    }

    private void HandleInputs()
    {
        //lookAroundAction.performed += context => lookAroundVector = context.ReadValue<Vector2>();
        //lookAroundAction.canceled += context => horizontalInput = 0;
    }


    void FixedUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        targetPos = carTransform.TransformPoint(moveOffset);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothness * Time.deltaTime);
    }

    void HandleRotation()
    {
        var direction = carTransform.position - transform.position;
        var rotation = new Quaternion();

        rotation = Quaternion.LookRotation(direction + rotOffset, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSmoothness * Time.deltaTime);
    }

    private void SetPositionToCarPosition()
    {
        transform.position = targetPos;
        transform.rotation = carTransform.rotation;
    }
}
