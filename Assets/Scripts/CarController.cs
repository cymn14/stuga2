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
    private float idleBrakeTorque = 100f;

    [SerializeField]
    private float tresholdForDriftingParticles = 0.8f;

    [SerializeField]
    private float minSpeedForDriftParticles = 0.8f;

    [SerializeField]
    private float driftParticleEmissionRateOverTime = 100f;

    [SerializeField]
    private Vector3 _centerOfMass;

    [SerializeField]
    private List<Wheel> wheels;

    [SerializeField]
    private float minSpeedForEngineSound;

    [SerializeField]
    private float minPitch;

    [SerializeField]
    private float maxPitch;

    [SerializeField]
    private AudioSource engineSound;

    [SerializeField]
    private AudioSource engineMaxSound;

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
    private InputAction moveAction;
    private InputAction steerAction;
    private PlayerInput playerInput;
    private GameController gameController;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private double speed = 0;
    private float moveInput; // Range from -1 to 1, -1 -> backward, 1 -> forward
    private float steerInput; // Range from -1 to 1, -1 -> left, 1 -> right

    private bool isDrifting = false;
    private int movingDirection = 0;

    private double pitchFromCar;

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
        EngineSound();

        if (gameController.getIsLevelRunning())
        {
            UpdateSpeed();
            Move();
            Steer();
            AnimateWheels();
            UpdateIsDrifting();
            UpdateMovingDirection();

            if (speed >= maxSpeed)
            {
                gameController.showMaxSpeedIndicator();
            }
            else
            {
                gameController.hideMaxSpeedIndicator();
            }
        }
    }

    private void UpdateIsDrifting()
    {
        bool localIsDrifting = false;

        if (speed >= minSpeedForDriftParticles && moveInput > 0.1 && IsMovingForward())
        {
            Vector3 carVelocity = carRigidbody.velocity;
            Vector3 carVelocityHorizontal = new Vector3(carVelocity.x, 0f, carVelocity.z);

            Vector3 carForward = transform.forward;
            Vector3 carForwardHorizontal = new Vector3(carForward.x, 0f, carForward.z);

            float lateralVelocityMagnitude = Vector3.Dot(carVelocityHorizontal, Vector3.Cross(carForwardHorizontal, Vector3.up));
            float forwardVelocityMagnitude = carVelocityHorizontal.magnitude;
            float lateralToForwardRatio = Mathf.Abs(lateralVelocityMagnitude / forwardVelocityMagnitude);

            localIsDrifting = lateralToForwardRatio > tresholdForDriftingParticles;
        }

        isDrifting = localIsDrifting;
    }

    private void UpdateSpeed()
    {
        speed = Math.Round(carRigidbody.velocity.magnitude, 2);
    }

    private void InitializeVariables()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        steerAction = playerInput.actions["Steer"];

        carRigidbody = GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = _centerOfMass;

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void HandleInputs()
    {
        moveAction.performed += context => moveInput = context.ReadValue<float>();
        moveAction.canceled += context => moveInput = 0;
        steerAction.performed += context => steerInput = context.ReadValue<float>();
        steerAction.canceled += context => steerInput = 0;
    }

    private void Move()
    {
        float frontAxelBrakeTorque = 0;
        float rearAxelBrakeTorque = 0;

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

        foreach (var wheel in wheels)
        {
            bool isWheelGrounded = IsWheelGrounded(wheel);

            wheel.wheelCollider.motorTorque = speed <= maxSpeed && isWheelGrounded ? moveInput * motorTorque : 0;

            if (wheel.axel == Axel.Front)
            {
                wheel.wheelCollider.brakeTorque = frontAxelBrakeTorque;
            }

            if (wheel.axel == Axel.Rear)
            {
                wheel.wheelCollider.brakeTorque = rearAxelBrakeTorque;

                var emission = wheel.smokeParticle.emission;

                if (isDrifting && isWheelGrounded)
                {
                    emission.rateOverTime = moveInput * driftParticleEmissionRateOverTime;
                }
                else
                {
                    emission.rateOverTime = 0f;
                }
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

    private void UpdateMovingDirection()
    {
        float dotP = Vector3.Dot(transform.forward.normalized, carRigidbody.velocity.normalized);

        if (dotP > 0.5f)
        {
            movingDirection = 1;
        }
        else if (dotP < -0.5f)
        {
            movingDirection = -1;
        }
        else
        {
            movingDirection = 0;
        }
    }

    public bool IsMovingForward()
    {
        return movingDirection == 1;
    }

    public bool IsMovingBackward()
    {
        return movingDirection == -1;
    }

    public bool IsStandingStill()
    {
        return movingDirection == 0;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "PlaneOfDoom")
        {
            gameController.FellDown();
        }
    }

    public void Reset()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        SetPositionToStartPosition();
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

    private bool IsWheelGrounded(Wheel wheel)
    {
        WheelHit hit;
        bool isGrounded = wheel.wheelCollider.GetGroundHit(out hit);
        return isGrounded;
    }

    private bool AreAllWheelsGrounded()
    {
        foreach (var wheel in wheels)
        {
            if (!IsWheelGrounded(wheel))
            {
                return false;
            }
        }


        return true;
    }

    private void EngineSound()
    {
        pitchFromCar = speed / 60f;

        if (!AreAllWheelsGrounded())
        {
            engineSound.pitch = minPitch + moveInput / 3;
        }
        else
        {
            if (speed < minSpeedForEngineSound)
            {
                engineSound.pitch = minPitch;
            }
            if (speed > minSpeedForEngineSound && speed < maxSpeed)
            {
                engineSound.pitch = minPitch + (float)pitchFromCar;
            }
        }

        //if (isDrifting)
        //{
        //    if (!engineMaxSound.isPlaying)
        //    {
        //        engineMaxSound.Play();
        //    }

        //} else
        //{
        //    if (engineMaxSound.isPlaying)
        //    {
        //        engineMaxSound.Stop();
        //    }
        //}
    }

    public void StartCar()
    {
        engineSound.Play();
    }

    public void TurnOffCar()
    {
        engineSound.Stop();
    }
}