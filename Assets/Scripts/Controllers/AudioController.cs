using UnityEngine;

/// <summary>
/// Controls the audio of the game.
/// </summary>
[RequireComponent(typeof(CarController))]
public class AudioController : MonoBehaviour
{
    public enum EngineAudioOptions
    {
        Simple,
        FourChannel
    }

    [Header("Engine Sound Style")]
    public EngineAudioOptions engineSoundStyle = EngineAudioOptions.FourChannel;

    [Header("Simple Mode Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Four Channel Clips")]
    public AudioClip lowAccelClip;
    public AudioClip lowDecelClip;
    public AudioClip highAccelClip;
    public AudioClip highDecelClip;
    public AudioClip skidClip;

    [Header("Pitch Settings")]
    public float pitchMultiplier = 1f;
    public float lowPitchMin = 1f;
    public float lowPitchMax = 6f;
    public float highPitchMultiplier = 0.25f;

    [Header("Rolloff & Doppler Settings")]
    public float maxRolloffDistance = 500f;
    public float dopplerLevel = 1f;
    public bool useDoppler = true;

    [Header("Input Controller")]
    [SerializeField] private InputDataController input;

    private AudioSource lowAccel;
    private AudioSource lowDecel;
    private AudioSource highAccel;
    private AudioSource highDecel;
    private AudioSource skidSource;

    private bool startedSound = false;
    private CarController carController;

    private int gear = 1;
    private float gearChangingSpeed = 1f;
    private float simulatedPitch = 1.0f;

    void Start()
    {
        carController = GetComponent<CarController>();
        if (input == null)
        {
            input = GetComponent<InputDataController>();
        }
    }

    void StartSound()
    {
        if (engineSoundStyle == EngineAudioOptions.Simple)
        {
            if (audioSource != null)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                highAccel = SetUpEngineAudioSource(highAccelClip != null ? highAccelClip : lowAccelClip);
            }
        }
        else
        {
            if (lowAccelClip != null) lowAccel = SetUpEngineAudioSource(lowAccelClip);
            if (lowDecelClip != null) lowDecel = SetUpEngineAudioSource(lowDecelClip);
            if (highAccelClip != null) highAccel = SetUpEngineAudioSource(highAccelClip);
            if (highDecelClip != null) highDecel = SetUpEngineAudioSource(highDecelClip);
        }

        if (skidClip != null)
        {
            skidSource = SetUpEngineAudioSource(skidClip);
            skidSource.volume = 0;
        }

        startedSound = true;
    }

    void StopSound()
    {
        if (engineSoundStyle == EngineAudioOptions.Simple && audioSource != null)
        {
            audioSource.Stop();
        }

        foreach (var source in GetComponents<AudioSource>())
        {
            if (source != audioSource)
            {
                Destroy(source);
            }
        }

        startedSound = false;
    }

    void Update()
    {
        float camDist = (Camera.main != null) ? (Camera.main.transform.position - transform.position).sqrMagnitude : 0f;
        bool isNear = camDist < maxRolloffDistance * maxRolloffDistance;

        if (startedSound && !isNear)
        {
            StopSound();
        }
        else if (!startedSound && isNear)
        {
            StartSound();
        }

        if (startedSound)
        {
            // Gear and pitch simulation
            ChangeGears();
            gearChangingSpeed = ChangeGearSpeed(gear);
            if (input != null && input.MoveInput > 0)
            {
                if (simulatedPitch > 2.5f)
                {
                    simulatedPitch = 2.5f;
                }
                simulatedPitch += gearChangingSpeed * Time.deltaTime;
            }
            else
            {
                DecreaseGears();
                if (simulatedPitch < 1.0f)
                {
                    simulatedPitch = 1.0f;
                }
                simulatedPitch -= 0.5f * Time.deltaTime;
            }

            if (engineSoundStyle == EngineAudioOptions.Simple)
            {
                if (audioSource != null)
                {
                    audioSource.pitch = simulatedPitch;
                    audioSource.volume = 1f;
                }
                else if (highAccel != null)
                {
                    highAccel.pitch = simulatedPitch * pitchMultiplier * highPitchMultiplier;
                    highAccel.volume = 1f;
                    highAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
                }
            }
            else
            {
                float normalizedRPM = Mathf.Clamp01((simulatedPitch - 1.0f) / 1.5f);
                float pitch = ULerp(lowPitchMin, lowPitchMax, normalizedRPM);

                if (lowAccel != null) lowAccel.pitch = pitch * pitchMultiplier;
                if (lowDecel != null) lowDecel.pitch = pitch * pitchMultiplier;
                if (highAccel != null) highAccel.pitch = pitch * highPitchMultiplier * pitchMultiplier;
                if (highDecel != null) highDecel.pitch = pitch * highPitchMultiplier * pitchMultiplier;

                float accFade = Mathf.Clamp01(input != null ? input.MoveInput : 0f);
                float decFade = 1f - accFade;

                float highFade = Mathf.InverseLerp(0.2f, 0.8f, normalizedRPM);
                float lowFade = 1f - highFade;

                highFade = 1f - ((1f - highFade) * (1f - highFade));
                lowFade = 1f - ((1f - lowFade) * (1f - lowFade));
                accFade = 1f - ((1f - accFade) * (1f - accFade));
                decFade = 1f - ((1f - decFade) * (1f - decFade));

                if (lowAccel != null) lowAccel.volume = lowFade * accFade;
                if (lowDecel != null) lowDecel.volume = lowFade * decFade;
                if (highAccel != null) highAccel.volume = highFade * accFade;
                if (highDecel != null) highDecel.volume = highFade * decFade;

                if (lowAccel != null) lowAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
                if (lowDecel != null) lowDecel.dopplerLevel = useDoppler ? dopplerLevel : 0;
                if (highAccel != null) highAccel.dopplerLevel = useDoppler ? dopplerLevel : 0;
                if (highDecel != null) highDecel.dopplerLevel = useDoppler ? dopplerLevel : 0;
            }

            if (skidSource != null && input != null && carController != null)
            {
                bool isBraking = false;
                if (carController.wheels != null)
                {
                    foreach (var wheel in carController.wheels)
                    {
                        if (wheel.wheelCollider != null && wheel.wheelCollider.isGrounded && input.BrakeInput != 0 && carController.carRb != null && carController.carRb.linearVelocity.magnitude >= 10f)
                        {
                            isBraking = true;
                            break;
                        }
                    }
                }
                skidSource.volume = isBraking ? 0.8f : 0f;
                skidSource.pitch = isBraking ? Mathf.Lerp(0.8f, 1.3f, carController.carRb.linearVelocity.magnitude / 50f) : 1f;
                skidSource.dopplerLevel = useDoppler ? dopplerLevel : 0;
            }
        }
    }

    AudioSource SetUpEngineAudioSource(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0;
        source.loop = true;

        source.time = Random.Range(0f, clip.length);
        source.Play();
        source.minDistance = 5;
        source.maxDistance = maxRolloffDistance;
        source.dopplerLevel = 0;
        return source;
    }

    float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }

    private float ChangeGearSpeed(int gear)
    {
        return gear switch
        {
            1 => 0.4f,
            2 => 0.3f,
            3 => 0.2f,
            4 => 0.1f,
            _ => 1f,
        };
    }

    private void ChangeGears()
    {
        if (simulatedPitch > 2.1f && gear == 1)
        {
            gear = 2;
            simulatedPitch = 1.4f;
        }
        else if (simulatedPitch > 2.2f && gear == 2)
        {
            gear = 3;
            simulatedPitch = 1.6f;
        }
        else if (simulatedPitch > 2.3f && gear == 3)
        {
            gear = 4;
            simulatedPitch = 1.7f;
        }
    }

    private void DecreaseGears()
    {
        if (simulatedPitch < 1.6f && gear == 4)
        {
            gear = 3;
            simulatedPitch = 1.8f;
        }
        else if (simulatedPitch < 1.4f && gear == 3)
        {
            gear = 2;
            simulatedPitch = 1.6f;
        }
        else if (simulatedPitch < 1.2f && gear == 2)
        {
            gear = 1;
            simulatedPitch = 1.3f;
        }
    }
}