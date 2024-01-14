using UnityEngine;

/// <summary>
/// Controls what type of camera the player chooses
/// </summary>
public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] 
    private Transform target;

    [SerializeField] 
    private Vector3 offset;

    private readonly float smoothSpeed = 0.125f;

    void FixedUpdate()
    {
        CameraFollow();
    }

    private void CameraFollow()
    {
        var desiredPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.LookAt(target);
    }
}
