using UnityEngine;
using System.Collections.Generic;
using System;

public class CarController : MonoBehaviour
{
    public Transform frontLeftBrakesTransform, frontRightBreaksTransform;
    private Vector3 localAngle;

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

    public List<Wheel> wheels;

    private float moveInput;
    private float steerInput;
    private float brakeInput;

    public int maxAcceleration = 30;
    public int brakeAcceleration = 1000;
    public int maxSteerAngle = 35;
    public int driveSpeed = 600;

    public Rigidbody carRb;
    public Joystick joystick;
    private InputDataController inputData;

    //private CarLights carLights;

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
        //carRb = GetComponent<Rigidbody>();
        //carLights = GetComponent<CarLights>();
        inputData = GetComponent<InputDataController>();
    }

    void Update()
    {
        GetInput();
        AnimateWheels();
        WheelEffects();
    }

    void FixedUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    private void LateUpdate()
    {
        SteerBrakes();
    }
    private void GetInput()
    {
        moveInput = inputData.MoveInput;
        steerInput = inputData.SteerInput;
        brakeInput = inputData.BrakeInput;
        if (joystick.isActiveAndEnabled)
        {
            moveInput = joystick.Vertical;
            steerInput = joystick.Horizontal;
        }
    }
    public void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * driveSpeed * maxAcceleration;
        }
    }

    public void Steer()
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
            //carLights.isBackLightOn = true;
            //carLights.OperateBackLights();
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
            //carLights.isBackLightOn = false;
            //carLights.OperateBackLights();
        }
    }

    void AnimateWheels()
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

    void WheelEffects()
    {
        foreach (var wheel in wheels)
        {
            if (brakeInput != 0 && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded == true && carRb.velocity.magnitude >= 10.0f)
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                wheel.smokeParticle.Emit(1);
            }
            else wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
        }
    }
}