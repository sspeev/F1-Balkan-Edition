using UnityEngine;
using UnityEngine.InputSystem;

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
        if (animation == null) return;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.nKey.wasPressedThisFrame)
            {
                animation.clip = rotationToNight;
                animation.Play();
            }
            if (Keyboard.current.mKey.wasPressedThisFrame) 
            {
                animation.clip = rotationToDay;
                animation.Play();
            }
        }
    }
}
