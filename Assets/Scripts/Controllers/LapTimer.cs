using TMPro;
using UnityEngine;

/// <summary>
/// Here is implemented the timer on the top left side of the interface
/// </summary>
public class LapTimer : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timerText;

    private float currentTime;

    public string TimeToPost { get; set; }

    private bool isFormationLapEnded = false;

    void Start()
    {
        currentTime = 0f;
    }

    void Update()
    {
        if (isFormationLapEnded)
        {
            currentTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        // Calculate minutes, seconds, and milliseconds
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        float milliseconds = (currentTime % 1) * 1000;

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isFormationLapEnded)
        {
            TimeToPost = timerText.text;
            UpdateTimerText();
        }
        else isFormationLapEnded = true;

    }
    private void OnTriggerStay(Collider other)
    {
        currentTime = 0f;
    }
}
