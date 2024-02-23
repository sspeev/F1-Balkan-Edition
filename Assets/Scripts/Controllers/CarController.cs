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

    void Start()
    {
        inputData = GetComponent<InputDataController>();
        carDTO = new()
        {
            CarBrand = carBrand.ToString(),
            Model = carModel.ToString(),
            Power = DriveSpeed.ToString()
        };
    }

    async void Update()
    {
        GetInput();
        await AnimateWheels();
        await WheelEffects();
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
<<<<<<< HEAD
    //private float forward;
    //private float backward;
    //private float steer;
    //private float left;
    //private float right;
    private void GetInput()
    {
        moveInput = inputData.MoveInput;
        //backward = inputData.BrakeInput;
        steerInput = inputData.SteerInput;
        //left = inputData.LeftInput;
        //right = inputData.RightInput;
        Debug.Log($"Move: {moveInput}");
        //steerInput = inputData.SteerInput;
        //steerInput = inputData.LeftInput - inputData.RightInput;
        Debug.Log($"Steer: {steerInput}");

        //if (forward > backward)
        //{
        //    moveInput = 1;
        //}
        //else if (backward > forward)
        //{
        //    moveInput = -1;
        //}
        //else moveInput = 0;

        //if (left > right)
        //{
        //    steerInput = -1;
        //}
        //else if (right > left)
        //{
        //    steerInput = 1;
        //}
        //else steerInput = 0;
=======
    private void GetInput()
    {
        moveInput = inputData.MoveInput;
        steerInput = inputData.SteerInput;
        brakeInput = inputData.BrakeInput;
>>>>>>> 8903b9192e3296442ad3891bea2cec01b84c3322
    }
    private void Move()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.wheelCollider != null)
            {
                wheel.wheelCollider.motorTorque = moveInput * driveSpeed * maxAcceleration * -1;
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
        localAngle = frontLeftBrakesTransform.localEulerAngles;
        localAngle.z = steerInput * maxSteerAngle;
        frontLeftBrakesTransform.localEulerAngles = localAngle;
        frontRightBreaksTransform.localEulerAngles = localAngle;
    }
    private void Brake()
    {
        if (brakeInput != 0 || moveInput == 0)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 600 * brakeAcceleration;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    private async Task AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.wheelCollider != null)
            {
                wheel.wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
                wheel.wheelModel.transform.SetPositionAndRotation(pos, rot);
                await Task.Yield();
            }
        }
    }

    private async Task WheelEffects()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.wheelCollider != null)
            {
                if (brakeInput != 0 && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded == true && carRb.velocity.magnitude >= 10.0f)
                {
                    wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                    wheel.smokeParticle.Emit(1);
                }
                else wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
                await Task.Yield();
            }
        }
    }
}