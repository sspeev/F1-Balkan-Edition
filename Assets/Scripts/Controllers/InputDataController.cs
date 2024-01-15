using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A script which works with the input system
/// </summary>
public class InputDataController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference moveInputAction;
    private float moveInput;

    [SerializeField]
    private InputActionReference steerInputAction;
    private float steerInput;

    [SerializeField]
    private InputActionReference brakeInputAction;
    private float brakeInput;

    public float MoveInput
    {
        get => moveInput;
        set
        {
            moveInput = value;
        }
    }
    public float SteerInput
    {
        get => steerInput;
        set
        {
            steerInput = value;
        }
    }
    public float BrakeInput
    {
        get => brakeInput;
        set
        {
            brakeInput = value;
        }
    }

    private void Awake()
    {
        moveInputAction.action.performed += Moving;
        steerInputAction.action.performed += Steering;
        brakeInputAction.action.performed += Breaking;
    }
    private void Update()
    {
        GetInput();
    }
    private void OnDestroy()
    {
        moveInputAction.action.performed -= Moving;
        steerInputAction.action.performed -= Steering;
        brakeInputAction.action.performed -= Breaking;
    }
    private void GetInput()
    {
        //moveInput = moveInputAction.action.ReadValue<float>();
        //steerInput = steerInputAction.action.ReadValue<float>();
        //brakeInput = brakeInputAction.action.ReadValue<float>();
    }
    private void Moving(InputAction.CallbackContext value)
    {
    }
    private void Steering(InputAction.CallbackContext value)
    {
    }
    private void Breaking(InputAction.CallbackContext value)
    {
    }
}
