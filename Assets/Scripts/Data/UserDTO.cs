using UnityEngine;

public class UserDTO : MonoBehaviour
{
    public UserDTO()
    {
        Car = new();
    }

    public string LapTime;

    public int Rank;

    public CarDTO Car;
}
