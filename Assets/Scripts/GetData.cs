using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GetData : MonoBehaviour
{
    [SerializeField]
    private GameObject car;

    private TMP_Text[] cars_text;

    [SerializeField]
    private GameObject lapTime;

    private TMP_Text[] lapTimes_text;

    [SerializeField]
    private GameObject track;

    private TMP_Text[] tracks_text;

    private bool isDataRecieved = false;

    private void Start()
    {
        cars_text = car.GetComponentsInChildren<TMP_Text>();
        lapTimes_text = lapTime.GetComponentsInChildren<TMP_Text>();
        tracks_text = track.GetComponentsInChildren<TMP_Text>();
        RefreshData();
    }
    public void RefreshData()
    {
        if (!isDataRecieved)
        {
            StartCoroutine(LoadCarData());
        }
    }
    private IEnumerator LoadCarData()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://localhost:7008/User");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received: " + request.downloadHandler.text);
            var dataCollection = JsonConvert.DeserializeObject<List<ExportUserDTO>>(request.downloadHandler.text);
            for (int i = 0; i < dataCollection.Count; i++)
            {
                cars_text[i].text = dataCollection[i].Car;
                lapTimes_text[i].text = dataCollection[i].LapTime;
                tracks_text[i].text = dataCollection[i].Track;
            }
        }
        else Debug.LogError("Error: " + request.error);
    }
}
