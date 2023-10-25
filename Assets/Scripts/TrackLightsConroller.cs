using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
