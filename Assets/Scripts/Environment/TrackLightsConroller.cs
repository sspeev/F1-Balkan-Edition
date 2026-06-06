using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Turns the track lights on and off
/// </summary>
//Obsolete
public class TrackLightsConroller : MonoBehaviour
{
    public Light[] lights;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.lKey.wasPressedThisFrame)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                if (lights[i] != null)
                {
                    lights[i].enabled = !lights[i].enabled;
                }
            }
        }
    }
}
