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
        DRS.clip = DRSenabled;
        DRS.Play();
        car.DriveSpeed = 1000;
    }
    private void OnTriggerExit(Collider other)
    {
        DRS.clip = DRSdisabled;
        DRS.Play();
        car.DriveSpeed = 600;
    }
}
