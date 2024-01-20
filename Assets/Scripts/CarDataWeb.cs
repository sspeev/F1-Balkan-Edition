using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Leguar.TotalJSON;

public class CarDataWeb : MonoBehaviour
{
    [SerializeField]
    private LapTimer currentTime = new();
    private void Update()
    {
        ProcessDataValidation();
    }

    private IEnumerator LoadCarData()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://localhost:7008/Car/get/2");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received: " + request.downloadHandler.text);
            var car = JsonConvert.DeserializeObject<CarDTO>(request.downloadHandler.text);
        }
        else Debug.LogError("Error: " + request.error);
    }

    private IEnumerator SendCarData()
    {
        // Create a user object to send to the API
        UserDTO userToSend = new()
        {
            LapTime = currentTime.TimeToPost,
            Rank = 1,
            Car = new()
            {
                Model = "Hubava",
                Power = "100"
            }
        };
        // Convert the user object to JSON
        string json = JSON.Serialize(userToSend).CreateString();

        // Setting up and sending the request
        UnityWebRequest request = UnityWebRequest.Post("http://localhost:5257/User/post", json, "application/json");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data sent successfully");
        }
        else Debug.LogError("Error: " + request.error);
    }

    private void ProcessDataValidation()
    {
        //StartCoroutine(LoadCarData());
        if (currentTime.TimeToPost != null)
            StartCoroutine(SendCarData());
        currentTime.TimeToPost = null;
    }
}
