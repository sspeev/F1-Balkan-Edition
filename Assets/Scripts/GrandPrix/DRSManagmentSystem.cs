using UnityEngine;

/// <summary>
/// Enables DRS depending on the triggered colider
/// </summary>
public class DRSManagmentSystem : MonoBehaviour
{
    [SerializeField]
    private AnimationClip DRSenabled;

    [SerializeField]
    private AnimationClip DRSdisabled;

    [SerializeField]
    private Animation DRS;

    [SerializeField]
    private CarController car;
    private void OnTriggerEnter(Collider other)
    {
        CarController enteringCar = other.GetComponentInParent<CarController>();
        if (enteringCar == null || enteringCar != car) return;

        DRS.clip = DRSenabled;
        DRS.Play();
        enteringCar.DriveSpeed = 1000;
    }
    private void OnTriggerExit(Collider other)
    {
        CarController enteringCar = other.GetComponentInParent<CarController>();
        if (enteringCar == null || enteringCar != car) return;

        DRS.clip = DRSdisabled;
        DRS.Play();
        enteringCar.DriveSpeed = enteringCar.BaseDriveSpeed;
    }
}
