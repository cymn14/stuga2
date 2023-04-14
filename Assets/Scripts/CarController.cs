using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

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
    private float idleBrakeTorque = 100f;

    [SerializeField]
    private Vector3 _centerOfMass;

    [SerializeField]
    private List<Wheel> wheels;

    [SerializeField]
    private TextMeshProUGUI textMeshProObject;

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
    private InputAction driftOnOffAction;
    private PlayerInput playerInput;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isBraking = false;
    private double speed = 0;
    private float moveInput; // Range from -1 to 1, -1 -> backward, 1 -> forward
    private float steerInput; // Range from -1 to 1, -1 -> left, 1 -> right
    private bool respawned = false;
    private bool driftingEnabled = true;

    private float rearWheelExtremumSlipNonDrift = 1.5f;
    private float rearWheelExtremumValueNonDrift = 1.5f;

    private float rearWheelExtremumSlipDrift = 1.5f;
    private float rearWheelExtremumValueDrift = 0.6f;

    private float rearWheelAsymptoteSlip = 0.5f;
    private float rearWheelAsymptoteValue = 0.75f;
    private float rearWheelStiffness = 2.4f;

    private void Awake()
    {
        InitializeVariables();
    }

    private void Start()
    {
        HandleInputs();
        RememberStartPosition();
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
        driftOnOffAction = playerInput.actions["DriftOnOff"];

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
        driftOnOffAction.performed += context => ToggleDriftMode();
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
        }
        else
        {
            carRigidbody.isKinematic = false;

            if ((IsMovingForward() && moveInput < 0) || (IsMovingBackward() && moveInput > 0))
            {
                frontAxelBrakeTorque = brakeTorque;
                rearAxelBrakeTorque = brakeTorque;
            }
            else if (moveInput == 0)
            {
                frontAxelBrakeTorque = idleBrakeTorque;
                rearAxelBrakeTorque = idleBrakeTorque;
            }

            if (isBraking)
            {
                rearAxelBrakeTorque = brakeTorque;
            }
        }


        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = speed <= maxSpeed && IsWheelGrounded(wheel) ? moveInput * motorTorque : 0;

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

    private static bool IsWheelGrounded(Wheel wheel)
    {
        WheelHit hit;
        bool isGrounded = wheel.wheelCollider.GetGroundHit(out hit);
        return isGrounded;
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

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "PlaneOfDoom")
        {
            Respawn();
        }

    }

    public void Respawn()
    {
        respawned = true;
        SetPositionToStartPosition();
    }

    public void ToggleDriftMode()
    {
        driftingEnabled = !driftingEnabled;

        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Rear)
            {
                WheelFrictionCurve wheelFrictionCurve = new WheelFrictionCurve();

                if (driftingEnabled)
                {
                    wheelFrictionCurve.extremumSlip = rearWheelExtremumSlipDrift;
                    wheelFrictionCurve.extremumValue = rearWheelExtremumValueDrift;

                }
                else
                {
                    wheelFrictionCurve.extremumSlip = rearWheelExtremumSlipNonDrift;
                    wheelFrictionCurve.extremumValue = rearWheelExtremumValueNonDrift;
                }

                wheelFrictionCurve.asymptoteSlip = rearWheelAsymptoteSlip;
                wheelFrictionCurve.asymptoteValue = rearWheelAsymptoteValue;
                wheelFrictionCurve.stiffness = rearWheelStiffness;

                wheel.wheelCollider.sidewaysFriction = wheelFrictionCurve;
            }
        }
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
}