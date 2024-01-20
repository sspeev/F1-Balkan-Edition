using UnityEngine;

/// <summary>
/// Plays smoke animation when the player drives through the gravel on the map
/// </summary>
public class GravelScript : MonoBehaviour
{
    [SerializeField]
    private CarController[] cars;

    private CarController currCar;

    private void OnTriggerStay(Collider other)
    {
        if (currCar == null)
        {
            foreach (var item in cars)
            {
                if (item.carDTO != null)
                {
                    currCar = item;
                }
            }
        }
        foreach (var wheel in currCar.wheels)
        {
            currCar.DriveSpeed = 10;
            if (wheel.wheelCollider.isGrounded == true && currCar.carRb.velocity.magnitude >= 5f)
            {
                var dirtParticleMainSettings = wheel.smokeParticle.main;
                dirtParticleMainSettings.startColor = Color.grey;
                wheel.smokeParticle.Emit(1);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        foreach (var wheel in currCar.wheels)
        {
            currCar.DriveSpeed = 600;
            var dirtParticleMainSettings = wheel.smokeParticle.main;
            dirtParticleMainSettings.startColor = Color.white;
        }
    }
}
