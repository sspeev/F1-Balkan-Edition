using Newtonsoft.Json;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LapTimer : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timerText;

    [SerializeField]
    private BoxCollider startFinishCollider;
    private float currentTime;

    public string TimeToPost { get; set; }

    void Start()
    {
        currentTime = 0f;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        UpdateTimerText();
    }
    private void OnTriggerEnter(Collider other)
    {
       TimeToPost = timerText.text;
        currentTime = 0f;
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
