using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    private float spinSpeed;

    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}