using System.Collections.Generic;
using UnityEngine;

public class LeftRightMovement : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> cars = new();

    [SerializeField]
    private new Camera camera;

    private int carCounter = 0;
    public void Right()
    {
        if(carCounter > cars.Count) 
            carCounter = 0;

        LookAtGameObj(camera, cars[carCounter]);
        carCounter++;
    }

    public void Left()
    {
        if (carCounter < 0)
            carCounter = cars.Count;

        LookAtGameObj(camera, cars[carCounter]);
        carCounter--;
    }

    private void LookAtGameObj(Camera camera, GameObject gameObj)
    {
        camera.transform.LookAt(gameObj.transform);
    }
}
