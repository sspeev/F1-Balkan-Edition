using UnityEngine;

/// <summary>
/// Plays smoke animation when the player drives through the gravel on the map
/// </summary>
public class GravelScript : MonoBehaviour
{
    [SerializeField]
    private CarController[] cars;

    private void OnTriggerStay(Collider other)
    {
        CarController enteringCar = other.GetComponentInParent<CarController>();
        if (enteringCar == null) return;

        enteringCar.DriveSpeed = 10;
        foreach (var wheel in enteringCar.wheels)
        {
            if (wheel.wheelCollider != null && wheel.smokeParticle != null)
            {
                if (wheel.wheelCollider.isGrounded == true && enteringCar.carRb != null && enteringCar.carRb.linearVelocity.magnitude >= 5f)
                {
                    var dirtParticleMainSettings = wheel.smokeParticle.main;
                    dirtParticleMainSettings.startColor = Color.grey;
                    wheel.smokeParticle.Emit(1);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        CarController enteringCar = other.GetComponentInParent<CarController>();
        if (enteringCar == null) return;

        enteringCar.DriveSpeed = enteringCar.BaseDriveSpeed;
        foreach (var wheel in enteringCar.wheels)
        {
            if (wheel.smokeParticle != null)
            {
                var dirtParticleMainSettings = wheel.smokeParticle.main;
                dirtParticleMainSettings.startColor = Color.white;
            }
        }
    }
}
