using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the input action asset
/// </summary>
public class InputActionController : MonoBehaviour
{
    public InputActionAsset asset;

    private void Awake()
    {
        asset.Enable();
    }
    private void OnDestroy()
    {
        asset.Disable();
    }
}
