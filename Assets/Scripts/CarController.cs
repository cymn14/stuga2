using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public float motorTorque = 10.0f;
    public float brakeTorque = 300.0f;
    public float maxSteerAngle = 30.0f;
    public float maxSpeed = 10.0f;
    public Vector3 _centerOfMass;
    public List<Wheel> wheels;

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffectObj;
        public ParticleSystem smokeParticle;
        public Axel axel;
    }

    private Rigidbody carRigidbody;
    private InputAction brakeAction;
    private InputAction moveAction;
    private InputAction steerAction;
    private PlayerInput playerInput;
    private bool isBraking = false;
    private double speed = 0;
    private float moveInput; // Range from -1 to 1, -1 -> backward, 1 -> forward
    private float steerInput; // Range from -1 to 1, -1 -> left, 1 -> right

    private void Awake()
    {
        InitializeVariables();
    }

    private void Start()
    {
        HandleInputs();

        Debug.Log("0: " + EaseInSine(0));
        Debug.Log("0.1: " + EaseInSine(0.1));
        Debug.Log("0.2: " + EaseInSine(0.2));
        Debug.Log("0.5: " + EaseInSine(0.5));
        Debug.Log("0.8: " + EaseInSine(0.8));
        Debug.Log("0.9: " + EaseInSine(0.9));
    }

    private void FixedUpdate()
    {
        UpdateSpeed();
        Move();
        Steer();
        AnimateWheels();
    }

    private void UpdateSpeed()
    {
        speed = Math.Round(carRigidbody.velocity.magnitude, 2);
    }

    private void InitializeVariables()
    {
        playerInput = GetComponent<PlayerInput>();
        brakeAction = playerInput.actions["Brake"];
        moveAction = playerInput.actions["Move"];
        steerAction = playerInput.actions["Steer"];

        carRigidbody = GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = _centerOfMass;
    }

    private void HandleInputs()
    {
        brakeAction.performed += context => isBraking = true;
        brakeAction.canceled += context => isBraking = false;
        moveAction.performed += context => moveInput = context.ReadValue<float>();
        moveAction.canceled += context => moveInput = 0;
        steerAction.performed += context => steerInput = context.ReadValue<float>();
        steerAction.canceled += context => steerInput = 0;
    }

    private void Move()
    {
        float currentBrakeTorgue = 0;

        if (isBraking || (IsMovingForward() && moveInput < 0) || (IsMovingBackward() && moveInput > 0))
        {
            currentBrakeTorgue = brakeTorque;
        }

        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = speed <= maxSpeed ? moveInput * motorTorque : 0;
            wheel.wheelCollider.brakeTorque = currentBrakeTorgue;
        }
    }

    private void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = (float)EaseInSine(steerInput) * maxSteerAngle;

                if (steerInput < 0)
                {
                    _steerAngle *= -1;
                }

                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    private void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    private double EaseInSine(double x)
    {
        return 1 - Math.Cos((x * Math.PI) / 2);
    }

    private int GetMovementDirection()
    {
        float dotP = Vector3.Dot(transform.forward.normalized, carRigidbody.velocity.normalized);
        if (dotP > 0.5f)
        {
            return 1;
        }
        else if (dotP < -0.5f)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public bool IsMovingForward()
    {
        return GetMovementDirection() == 1;
    }

    public bool IsMovingBackward()
    {
        return GetMovementDirection() == -1;
    }

    public bool IsStandingStill()
    {
        return GetMovementDirection() == 0;
    }
}
