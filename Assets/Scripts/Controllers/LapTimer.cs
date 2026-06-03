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

    public bool IsFormationLapEnded { get; set; } = false;

    void Start()
    {
        currentTime = 0f;
    }

    void Update()
    {
        if (IsFormationLapEnded)
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
        if (IsFormationLapEnded)
        {
            TimeToPost = timerText.text;
            UpdateTimerText();
        }
        else IsFormationLapEnded = true;

    }
    private void OnTriggerStay(Collider other)
    {
        currentTime = 0f;
    }
}
