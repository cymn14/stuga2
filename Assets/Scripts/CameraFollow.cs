using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public float moveSmoothness;
    public float rotSmoothness;

    public Vector3 moveOffset;
    public Vector3 rotOffset;

    public Transform carTarget;

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
    }

    private void InitializeVariables()
    {
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
        Vector3 targetPos = new Vector3();
        targetPos = carTarget.TransformPoint(moveOffset);

        transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothness * Time.deltaTime);
    }

    void HandleRotation()
    {
        var direction = carTarget.position - transform.position;
        var rotation = new Quaternion();

        rotation = Quaternion.LookRotation(direction + rotOffset, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSmoothness * Time.deltaTime);
    }
}
