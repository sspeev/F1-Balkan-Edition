//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CameraRotation : MonoBehaviour
//{
//    public float speed;
//    void Update()
//    {
//        transform.Rotate(Vector3.up * speed * Time.deltaTime);
//    }
//}

using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public float spinSpeed;

    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}
