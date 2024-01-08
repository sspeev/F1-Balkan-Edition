using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CarDataWeb : MonoBehaviour
{
    private string api = "https://localhost:7008/Car/get/6";

    [SerializeField]
    private Collider colider;

    private void Awake()
    {
       // StartCoroutine(LoadCarData(api));
    }
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(SendCar(api));
    }

    private IEnumerator LoadCarData(string api)
    {
        UnityWebRequest request = UnityWebRequest.Get("https://localhost:7008/Car/get/2");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received: " + request.downloadHandler.text);
            var car = JsonConvert.DeserializeObject<CarDTO>(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    private IEnumerator SendCar(string api)
    {
        // Create a user object to send to the API
        UserDTO userToSend = new();
        LapTimer currentTime = new();
        currentTime.TimeToPost = "01:34:63";
        userToSend.LapTime = currentTime.TimeToPost;

        // Convert the user object to JSON
        string jsonData = JsonUtility.ToJson(userToSend);

        // Set up the request
        UnityWebRequest request = new("https://localhost:7008/Car/post", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data sent successfully");
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
}
