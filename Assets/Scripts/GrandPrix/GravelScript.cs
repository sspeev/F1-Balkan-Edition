using UnityEngine;

/// <summary>
/// Plays smoke animation when the player drives through the gravel on the map
/// </summary>
public class GravelScript : MonoBehaviour
{
    [SerializeField]
    private CarController car;

    private void OnTriggerStay(Collider other)
    {
        foreach (var wheel in car.wheels)
        {
            car.DriveSpeed = 10;
            if (wheel.wheelCollider.isGrounded == true && car.carRb.velocity.magnitude >= 5f)
            {
                var dirtParticleMainSettings = wheel.smokeParticle.main;
                dirtParticleMainSettings.startColor = Color.grey;
                wheel.smokeParticle.Emit(1);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        foreach (var wheel in car.wheels)
        {
            car.DriveSpeed = 600;
            var dirtParticleMainSettings = wheel.smokeParticle.main;
            dirtParticleMainSettings.startColor = Color.white;
        }
    }
}
