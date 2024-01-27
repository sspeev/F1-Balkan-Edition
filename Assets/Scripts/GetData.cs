using Leguar.TotalJSON;
using Leguar.TotalJSON.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public class GetData : MonoBehaviour
{
    private bool isDataRecieved = false;
    private void Update()
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
            ParseJsonToObject(request.downloadHandler.text);
            
        }
        else Debug.LogError("Error: " + request.error);
    }
    private void ParseJsonToObject(string json)
    {
        var wrappedjsonArray = JsonUtility.FromJson<MyWrapper>(json);
    }

    [Serializable]
    private class MyWrapper
    {
        public List<UserDTO> objects;
    }
}