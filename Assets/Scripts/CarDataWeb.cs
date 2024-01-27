using Leguar.TotalJSON;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CarDataWeb : MonoBehaviour
{
    [SerializeField]
    private LapTimer currentTime;
    private Scene currScene;
    private string track;

    [SerializeField]
    private CarController[] cars;

    private void Start()
    {
        currScene = SceneManager.GetActiveScene();
        track = SceneChecker();
    }
    private void Update()
    {
        ProcessDataValidation();
    }

    private IEnumerator SendCarData()
    {

        // Create a user object to send to the API
        UserDTO userToSend = new()
        {
            LapTime = currentTime.TimeToPost,
            Track = track
        };
        foreach (var car in cars)
        {
            if (car.carDTO != null)
            {
                userToSend.Car = car.carDTO;
            }
        }
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
        if (currentTime.TimeToPost != null && currentTime.IsFormationLapEnded && currentTime.TimeToPost != "00:00:00")
            StartCoroutine(SendCarData());
        currentTime.TimeToPost = null;
    }

    private string SceneChecker()
    {
        string currCircuit = string.Empty;
        if (currScene.name == "GameScene")
        {
            currCircuit = "International Balkan Circuit";
        }
        else if (currScene.name == "TutorialTrack")
        {
            currCircuit = "TutorialTrack";
        }
        return currCircuit;
    }
}
