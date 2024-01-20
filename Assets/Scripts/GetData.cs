using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GetData : MonoBehaviour
{
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
}
