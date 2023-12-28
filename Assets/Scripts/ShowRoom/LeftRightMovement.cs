using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightMovement : MonoBehaviour
{
    public Transform camera;

    void Start()
    {
        camera.position = new Vector3(-17.5f, 1.3f, 45f);
    }

    void Update()
    {
    }

    public void Right()
    {
        camera.position = new Vector3(camera.position.x + 0.4f, camera.position.y, camera.position.z + 14f);
    }

    public void Left()
    {
        camera.position = new Vector3(camera.position.x - 0.4f, camera.position.y, camera.position.z - 14f);
    }
}