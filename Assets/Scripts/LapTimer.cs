using UnityEngine;
using UnityEngine.UI;

public class LapTimer : MonoBehaviour
{
    public Text timerText;
    private float currentTime;

    void Start()
    {
        currentTime = 0f;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        // Calculate minutes, seconds, and milliseconds
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        float milliseconds = (currentTime % 1) * 1000;

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
