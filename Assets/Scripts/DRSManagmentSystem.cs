using UnityEngine;

public class DRSManagmentSystem : MonoBehaviour
{
    public AnimationClip DRSenabled;
    public AnimationClip DRSdisabled;
    public Animation DRS;
    public CarController car;
    //private CarScript carSpeed;

    //[SerializeField]
    //private bool isDRSActive = false;

    //private void Update()
    //{
    //    if(Input.GetKey(KeyCode.Z))
    //    {
    //        if (!isDRSActive)
    //        {
    //            DRS.clip = DRSenabled;
    //            DRS.Play();
    //            isDRSActive = true;
    //        }
    //        else
    //        {
    //            DRS.clip = DRSdisabled;
    //            DRS.Play();
    //            isDRSActive = false;
    //        }
    //    }
    //}
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
