using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float spinSpeed;


    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}