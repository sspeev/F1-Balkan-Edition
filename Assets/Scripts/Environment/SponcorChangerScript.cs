using UnityEngine;

/// <summary>
/// Plays all of the animated sponsor panels on the map
/// </summary>
public class SponcorChangerScript : MonoBehaviour
{
    public new Animation animation;

    private void Start()
    {
        animation.Play();
    }
}
