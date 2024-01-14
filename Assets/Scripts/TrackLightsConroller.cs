using UnityEngine;

/// <summary>
/// Turns the track lights on and off
/// </summary>
//Obsolete
public class TrackLightsConroller : MonoBehaviour
{
    public Light[] lights;

    void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].enabled = !lights[i].enabled;
            }
        }
    }
}
