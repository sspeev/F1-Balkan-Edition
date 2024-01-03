using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Web : MonoBehaviour
{
    [SerializeField]
    private string api = "https://localhost:7008/Car/get/6";
    private void Awake()
    {
        StartCoroutine(GetData(api));
        StartCoroutine(PostData(api));
    }
    private IEnumerator GetData(string api)
    {
        //api = api + "Car/get/6";
        UnityWebRequest request = UnityWebRequest.Get("https://localhost:7008/Car/get/6");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    private IEnumerator PostData(string api)
    {
        api = api + "post";
        // Create a user object to send to the API
        UserDTO userToSend = new();
        LapTimer currentTime = new();
        userToSend.LapTime = currentTime.TimeToPost;

        // Convert the user object to JSON
        string jsonData = JsonUtility.ToJson(userToSend);

        // Set up the request
        UnityWebRequest request = new(api, "POST");
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
