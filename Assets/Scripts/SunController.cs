using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    public AnimationClip rotationToNight;
    public AnimationClip rotationToDay;
    public new Animation animation;
    void Update()
    {
        if(Input.GetKey(KeyCode.N))
        {
            animation.clip = rotationToNight;
            animation.Play();
        }
        if (Input.GetKey(KeyCode.M)) 
        {
            animation.clip = rotationToDay;
            animation.Play();
        }
    }
}
