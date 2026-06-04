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
        Corvette,
        Ford,
        Custom
    }

    public enum CarModel
    {
        F1Car,
        _911,
        ZR11,
        Mustang
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

        // Cache initial local rotations of front brake caliper bones to prevent Euler conversion drift / 360 spinning.
        if (frontLeftBrakesTransform != null)
        {
            initialLeftBrakeLocalRot = frontLeftBrakesTransform.localRotation;
        }
        if (frontRightBreaksTransform != null)
        {
            initialRightBrakeLocalRot = frontRightBreaksTransform.localRotation;
        }

        // Cache initial rotation offsets between the WheelCollider and the 3D wheel model mesh.
        // This corrects any pivot/import rotation mismatches (e.g. if the mesh starts rotated 90 degrees).
        if (wheels != null)
        {
            wheelRotationOffsets = new Quaternion[wheels.Count];
            for (int i = 0; i < wheels.Count; i++)
            {
                var wheel = wheels[i];
                if (wheel.wheelCollider != null && wheel.wheelModel != null)
                {
                    wheel.wheelCollider.GetWorldPose(out _, out Quaternion initColliderRot);
                    wheelRotationOffsets[i] = Quaternion.Inverse(initColliderRot) * wheel.wheelModel.transform.rotation;
                }
                else
                {
                    wheelRotationOffsets[i] = Quaternion.identity;
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

    private void Move()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.wheelCollider != null)
            {
                wheel.wheelCollider.motorTorque = moveInput * driveSpeed * maxAcceleration;
            }
        }
    }

    private void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
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
        if (brakeInput != 0 || moveInput == 0)
        {
            foreach (var wheel in wheels)
            {
                if (wheel.wheelCollider != null)
                {
                    wheel.wheelCollider.brakeTorque = 600 * brakeAcceleration;
                }
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                if (wheel.wheelCollider != null)
                {
                    wheel.wheelCollider.brakeTorque = 0;
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
                
                wheel.wheelModel.transform.SetPositionAndRotation(pos, correctiveRot);
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