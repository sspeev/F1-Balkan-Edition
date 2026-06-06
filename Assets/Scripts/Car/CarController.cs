using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

/// <summary>
/// The main controller which controls the movment of the cars and their animations
/// </summary>
public class CarController : MonoBehaviour
{
    [SerializeField] private Transform frontLeftBrakesTransform, frontRightBreaksTransform;
    private Vector3 localAngle;
    private Quaternion[] wheelRotationOffsets;
    private Vector3[] wheelLocalOffsets;
    private Quaternion initialLeftBrakeLocalRot;
    private Quaternion initialRightBrakeLocalRot;

    public enum Axel
    {
        Front,
        Rear
    }

    public enum CarBrand
    {
        Audi,
        Porsche,
        Mercedes
    }

    public enum CarModel
    {
        F1Car,
        _911,
        W211
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
    public List<Wheel> wheels;

    private float moveInput;
    private float steerInput;
    private float brakeInput;

    [SerializeField] private int maxAcceleration = 1;
    [SerializeField] private int brakeAcceleration = 1000;
    [SerializeField] private int maxSteerAngle = 35;
    [SerializeField] private int driveSpeed = 100;

    public Rigidbody carRb;
    [SerializeField] private Joystick joystick;
    private InputDataController inputData;
    internal CarDTO carDTO;

    [SerializeField]
    private CarBrand carBrand;

    [SerializeField]
    private CarModel carModel;

    public int DriveSpeed
    {
        get => driveSpeed;
        set
        {
            driveSpeed = value;
        }
    }

    public int BaseDriveSpeed { get; private set; }

    private int brainContr;

    // Realism Physics Settings
    private float[] gearRatios = { 3.50f, 2.80f, 2.30f, 1.90f, 1.60f, 1.30f, 1.00f, 0.85f };
    private float finalDriveRatio = 3.5f;
    private int currentGear = 0;
    private float currentRPM = 1000f;
    private float maxRPM = 7000f;
    private float idleRPM = 1000f;
    private float downforceCoeff = 0.5f;
    private bool isRWD = true;
    private float shiftDelayTimer = 0f;
    private const float shiftDelayDuration = 0.15f; // Duration of gear shift interruption

    public float CurrentRPM => currentRPM;
    public int CurrentGear => currentGear + 1; // 1-based gear
    public float MaxRPM => maxRPM;

    void Start()
    {
        BaseDriveSpeed = driveSpeed;
        inputData = GetComponent<InputDataController>();
        carDTO = new()
        {
            CarBrand = carBrand.ToString(),
            Model = carModel.ToString(),
            Power = DriveSpeed.ToString()
        };

        // Initialize model-specific settings for engine, gearbox, drivetrain and aerodynamics
        switch (carModel)
        {
            case CarModel.F1Car:
                gearRatios = new float[] { 3.78f, 2.82f, 2.17f, 1.76f, 1.45f, 1.22f, 1.04f, 0.90f }; // F1-style 8-speed
                finalDriveRatio = 4.0f;
                maxRPM = 12000f;
                idleRPM = 2000f;
                downforceCoeff = 4.5f;
                isRWD = true;
                break;
            case CarModel._911:
                gearRatios = new float[] { 3.91f, 2.29f, 1.58f, 1.18f, 0.94f, 0.79f, 0.67f }; // PDK 7-speed
                finalDriveRatio = 3.44f;
                maxRPM = 7500f;
                idleRPM = 900f;
                downforceCoeff = 0.8f;
                isRWD = true;
                break;
            case CarModel.W211:
                gearRatios = new float[] { 4.38f, 2.86f, 1.92f, 1.37f, 1.00f, 0.82f, 0.70f }; // 7G-Tronic 7-speed
                finalDriveRatio = 3.07f;
                maxRPM = 6000f;
                idleRPM = 700f;
                downforceCoeff = 0.2f;
                isRWD = true;
                break;
        }
        currentRPM = idleRPM;

        // Cache initial local rotations of front brake caliper bones to prevent Euler conversion drift / 360 spinning.
        if (frontLeftBrakesTransform != null)
        {
            initialLeftBrakeLocalRot = frontLeftBrakesTransform.localRotation;
        }
        if (frontRightBreaksTransform != null)
        {
            initialRightBrakeLocalRot = frontRightBreaksTransform.localRotation;
        }

        // Cache initial rotation offsets and position offsets between the WheelCollider and the 3D wheel model mesh.
        // This corrects any pivot/import rotation and position mismatches (e.g. if the mesh pivot is at the car's origin).
        if (wheels != null)
        {
            wheelRotationOffsets = new Quaternion[wheels.Count];
            wheelLocalOffsets = new Vector3[wheels.Count];
            for (int i = 0; i < wheels.Count; i++)
            {
                var wheel = wheels[i];
                if (wheel.wheelCollider != null && wheel.wheelModel != null)
                {
                    wheel.wheelCollider.GetWorldPose(out Vector3 initColliderPos, out Quaternion initColliderRot);
                    wheelRotationOffsets[i] = Quaternion.Inverse(initColliderRot) * wheel.wheelModel.transform.rotation;
                    wheelLocalOffsets[i] = Quaternion.Inverse(wheel.wheelModel.transform.rotation) * (initColliderPos - wheel.wheelModel.transform.position);
                }
                else
                {
                    wheelRotationOffsets[i] = Quaternion.identity;
                    wheelLocalOffsets[i] = Vector3.zero;
                }
            }
        }
    }

    void Update()
    {
        GetInput();
        AnimateWheels();
        WheelEffects();
    }

    private void FixedUpdate()
    {
        UpdateEngineAndGears();
        ApplyDownforce();
        Move();
        Steer();
        Brake();
    }

    private void LateUpdate()
    {
        if (carModel == CarModel.F1Car)
        {
            SteerBrakes();
        }
    }

    private void GetInput()
    {
        brainContr = PlayerPrefs.GetInt("brainContr");

        if (inputData != null)
        {
            moveInput = inputData.MoveInput;
            steerInput = inputData.SteerInput;
            brakeInput = inputData.BrakeInput;
        }
    }

    private void UpdateEngineAndGears()
    {
        if (shiftDelayTimer > 0f)
        {
            shiftDelayTimer -= Time.fixedDeltaTime;
        }

        float averageDrivingWheelRPM = 0f;
        int drivingWheelCount = 0;
        float wheelRadius = 0.36f;
        foreach (var w in wheels)
        {
            if ((!isRWD || w.axel == Axel.Rear) && w.wheelCollider != null)
            {
                averageDrivingWheelRPM += Mathf.Abs(w.wheelCollider.rpm);
                drivingWheelCount++;
                wheelRadius = w.wheelCollider.radius;
            }
        }
        if (drivingWheelCount > 0)
        {
            averageDrivingWheelRPM /= drivingWheelCount;
        }

        float totalRatio = gearRatios[currentGear] * finalDriveRatio;
        float targetRPM = averageDrivingWheelRPM * totalRatio;
        currentRPM = Mathf.Clamp(targetRPM, idleRPM, maxRPM);

        if (shiftDelayTimer <= 0f)
        {
            float speed = carRb != null ? carRb.linearVelocity.magnitude : 0f;
            
            // Only allow shifting up if the actual speed is at least 65% of the RPM-theoretical speed for the current gear.
            // This prevents rapid upshifting (and the resulting power-cut shift delay) during stationary wheelspin.
            float shiftUpMinSpeed = (maxRPM * 0.65f / 60f) * (2f * Mathf.PI * wheelRadius) / totalRatio;

            if (currentRPM > maxRPM * 0.92f && currentGear < gearRatios.Length - 1 && speed >= shiftUpMinSpeed)
            {
                currentGear++;
                shiftDelayTimer = shiftDelayDuration;
            }
            else if (currentRPM < maxRPM * 0.55f && currentGear > 0)
            {
                currentGear--;
                shiftDelayTimer = shiftDelayDuration;
            }
        }
    }

    private void ApplyDownforce()
    {
        if (carRb != null)
        {
            float speed = carRb.linearVelocity.magnitude;
            float downforce = downforceCoeff * speed * speed;
            carRb.AddForce(-transform.up * downforce);
        }
    }

    private void Move()
    {
        if (shiftDelayTimer > 0f)
        {
            foreach (var w in wheels)
            {
                if (w.wheelCollider != null)
                {
                    w.wheelCollider.motorTorque = 0f;
                }
            }
            return;
        }

        float rpmNormalized = currentRPM / maxRPM;
        float torqueCurveMultiplier = Mathf.Clamp01(1.0f - Mathf.Pow(rpmNormalized - 0.7f, 2f) * 2f);
        float engineTorque = moveInput * driveSpeed * maxAcceleration * 20f * torqueCurveMultiplier;
        float totalRatio = gearRatios[currentGear] * finalDriveRatio;
        float wheelTorque = engineTorque * totalRatio;

        float tcsFactor = 1.0f;
        foreach (var w in wheels)
        {
            if (w.axel == Axel.Rear && w.wheelCollider != null)
            {
                if (w.wheelCollider.GetGroundHit(out WheelHit hit))
                {
                    float forwardSlip = Mathf.Abs(hit.forwardSlip);
                    if (forwardSlip > 0.35f)
                    {
                        tcsFactor = Mathf.Min(tcsFactor, Mathf.Clamp01(1f - (forwardSlip - 0.35f) * 3f));
                    }
                }
            }
        }
        tcsFactor = Mathf.Clamp(tcsFactor, 0.1f, 1.0f);
        float finalTorque = wheelTorque * tcsFactor;

        foreach (var w in wheels)
        {
            if (w.wheelCollider != null)
            {
                if (!isRWD || w.axel == Axel.Rear)
                {
                    w.wheelCollider.motorTorque = finalTorque;
                }
                else
                {
                    w.wheelCollider.motorTorque = 0f;
                }
            }
        }
    }

    private void Steer()
    {
        float speed = carRb != null ? carRb.linearVelocity.magnitude : 0f;
        float speedFactor = Mathf.Clamp01(1f - (speed / 60f) * 0.7f);
        float targetSteerAngle = steerInput * maxSteerAngle * speedFactor;

        foreach (var w in wheels)
        {
            if (w.axel == Axel.Front && w.wheelCollider != null)
            {
                w.wheelCollider.steerAngle = Mathf.Lerp(w.wheelCollider.steerAngle, targetSteerAngle, 0.6f);
            }
        }
    }

    private void SteerBrakes()
    {
        WheelCollider frontLeftCollider = null;
        WheelCollider frontRightCollider = null;

        if (wheels != null)
        {
            foreach (var wheel in wheels)
            {
                if (wheel.axel == Axel.Front && wheel.wheelCollider != null)
                {
                    if (frontLeftCollider == null)
                    {
                        frontLeftCollider = wheel.wheelCollider;
                    }
                    else if (frontRightCollider == null)
                    {
                        frontRightCollider = wheel.wheelCollider;
                        break;
                    }
                }
            }
        }

        // Apply steering rotation around local Z-axis relative to the initial cached local rotation.
        // This avoids Euler angles gimbal lock / flipping bugs.
        if (frontLeftBrakesTransform != null && frontLeftCollider != null)
        {
            frontLeftBrakesTransform.localRotation = initialLeftBrakeLocalRot * Quaternion.AngleAxis(frontLeftCollider.steerAngle, Vector3.forward);
        }

        if (frontRightBreaksTransform != null && frontRightCollider != null)
        {
            frontRightBreaksTransform.localRotation = initialRightBrakeLocalRot * Quaternion.AngleAxis(frontRightCollider.steerAngle, Vector3.forward);
        }
    }

    private void Brake()
    {
        float rollingResistance = 15f;
        float engineBrakeTorque = 30f;

        foreach (var w in wheels)
        {
            if (w.wheelCollider != null)
            {
                if (brakeInput != 0f)
                {
                    w.wheelCollider.brakeTorque = brakeInput * 1500f * brakeAcceleration;
                    w.wheelCollider.motorTorque = 0f;
                }
                else if (moveInput == 0f)
                {
                    float coastingBrake = rollingResistance;
                    if (!isRWD || w.axel == Axel.Rear)
                    {
                        coastingBrake += engineBrakeTorque;
                    }
                    w.wheelCollider.brakeTorque = coastingBrake;
                    w.wheelCollider.motorTorque = 0f;
                }
                else
                {
                    w.wheelCollider.brakeTorque = 0f;
                }
            }
        }
    }

    private void AnimateWheels()
    {
        if (wheels == null) return;

        for (int i = 0; i < wheels.Count; i++)
        {
            var wheel = wheels[i];
            if (wheel.wheelCollider != null && wheel.wheelModel != null)
            {
                wheel.wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
                
                // Apply the cached pivot offset correction to the collider's rotation
                Quaternion correctiveRot = rot;
                if (wheelRotationOffsets != null && i < wheelRotationOffsets.Length)
                {
                    correctiveRot = rot * wheelRotationOffsets[i];
                }

                // Apply the cached position offset correction to align the visual center with the collider
                Vector3 correctivePos = pos;
                if (wheelLocalOffsets != null && i < wheelLocalOffsets.Length)
                {
                    correctivePos = pos - correctiveRot * wheelLocalOffsets[i];
                }
                
                wheel.wheelModel.transform.SetPositionAndRotation(correctivePos, correctiveRot);
            }
        }
    }

    private void WheelEffects()
    {
        if (wheels == null) return;

        foreach (var wheel in wheels)
        {
            if (wheel.wheelCollider != null)
            {
                bool isBrakingRear = brakeInput != 0 && 
                                     wheel.axel == Axel.Rear && 
                                     wheel.wheelCollider.isGrounded && 
                                     carRb != null && 
                                     carRb.linearVelocity.magnitude >= 10.0f;

                if (wheel.wheelEffectObj != null)
                {
                    var trail = wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>();
                    if (trail != null)
                    {
                        trail.emitting = isBrakingRear;
                    }
                }

                if (isBrakingRear && wheel.smokeParticle != null)
                {
                    wheel.smokeParticle.Emit(1);
                }
            }
        }
    }
}