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

    private int brainContr;

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
    public float LeftInput { get; set; }

    public float RightInput { get; set; }

    public float BrakeInput
    {
        get => brakeInput;
        set
        {
            brakeInput = value;
        }
    }

    private Joystick joystick;
    private BrakeButton brakeButton;

    private void Awake()
    {
        moveInputAction.action.performed += Moving;
        steerInputAction.action.performed += Steering;
        brakeInputAction.action.performed += Breaking;
    }
    private void Start()
    {
        // Find the active Joystick in the scene (e.g. Fixed Joystick)
        joystick = FindObjectOfType<Joystick>();

        // Dynamically find the Brake GameObject in the scene and attach the BrakeButton component
        GameObject brakeGo = GameObject.Find("Brake");
        if (brakeGo != null)
        {
            brakeButton = brakeGo.GetComponent<BrakeButton>();
            if (brakeButton == null)
            {
                brakeButton = brakeGo.AddComponent<BrakeButton>();
            }

            // Remove the incorrect onClick event listener that takes the player to the menu scene
            var btn = brakeGo.GetComponent<UnityEngine.UI.Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
            }
        }
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
        brainContr = PlayerPrefs.GetInt("brainContr");
        if (brainContr == 0)
        {
            // If the UI Joystick is active in the hierarchy, read from mobile controls
            if (joystick != null && joystick.gameObject.activeInHierarchy)
            {
                steerInput = joystick.Horizontal;
                moveInput = joystick.Vertical;
                brakeInput = (brakeButton != null && brakeButton.IsPressed) ? 1f : 0f;
            }
            else
            {
                // Fallback to keyboard/gamepad inputs
                moveInput = moveInputAction.action.ReadValue<float>();
                steerInput = steerInputAction.action.ReadValue<float>();
                brakeInput = brakeInputAction.action.ReadValue<float>();
            }
        }
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
