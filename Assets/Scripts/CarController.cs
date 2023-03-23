using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private float motorTorque = 10.0f;
    [SerializeField]
    private float brakeTorque = 300.0f;
    [SerializeField]
    private float maxSteerAngle = 30.0f;
    [SerializeField]
    private float maxSpeed = 10.0f;
    [SerializeField]
    private Vector3 _centerOfMass;
    [SerializeField]
    private List<Wheel> wheels;
    [SerializeField]
    private Transform startPosition;

    private enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    private struct Wheel
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
    private InputAction respawnAction;
    private PlayerInput playerInput;
    private bool isBraking = false;
    private double speed = 0;
    private float moveInput; // Range from -1 to 1, -1 -> backward, 1 -> forward
    private float steerInput; // Range from -1 to 1, -1 -> left, 1 -> right
    private bool respawned = false;

    private void Awake()
    {
        InitializeVariables();
    }

    private void Start()
    {
        HandleInputs();
        SetPositionToStartPosition();
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
        respawnAction = playerInput.actions["Respawn"];

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
        respawnAction.performed += context => Respawn();
    }

    private void Move()
    {
        float frontAxelBrakeTorque = 0;
        float rearAxelBrakeTorque = 0;

        if (respawned)
        {
            frontAxelBrakeTorque = float.MaxValue;
            rearAxelBrakeTorque = float.MaxValue;
            carRigidbody.isKinematic = true;
            respawned = false;
        } else
        {
            carRigidbody.isKinematic = false;

            if ((IsMovingForward() && moveInput < 0) || (IsMovingBackward() && moveInput > 0))
            {
                frontAxelBrakeTorque = brakeTorque;
                rearAxelBrakeTorque = brakeTorque;
            }

            if (isBraking)
            {
                rearAxelBrakeTorque = brakeTorque;
            }
        }

        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = speed <= maxSpeed ? moveInput * motorTorque : 0;

            if (wheel.axel == Axel.Front)
            {
                wheel.wheelCollider.brakeTorque = frontAxelBrakeTorque;
            }

            if (wheel.axel == Axel.Rear)
            {
                wheel.wheelCollider.brakeTorque = rearAxelBrakeTorque;
            }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlaneOfDoom")
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        respawned = true;
        SetPositionToStartPosition();
    }

    private void SetPositionToStartPosition()
    {
        transform.position = startPosition.position;
        transform.rotation = startPosition.rotation;
    }
}
