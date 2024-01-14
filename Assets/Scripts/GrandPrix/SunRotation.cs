using UnityEngine;

/// <summary>
/// Changes the day to night and the night to day
/// </summary>
public class SunRotation : MonoBehaviour
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
