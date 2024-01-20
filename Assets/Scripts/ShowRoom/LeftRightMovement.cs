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
        if(camera.position.z == -9f)
        {
            camera.position = new Vector3(camera.position.x, camera.position.y, 45f);
        }
        else
        {
            camera.position = new Vector3(camera.position.x, camera.position.y, camera.position.z - 18f);
        }
    }

    public void Left()
    {
        if(camera.position.z == 45f)
        {
            camera.position = new Vector3(camera.position.x, camera.position.y, -9f);
        }
        else
        {
            camera.position = new Vector3(camera.position.x, camera.position.y, camera.position.z + 18f);
        }
    }
}