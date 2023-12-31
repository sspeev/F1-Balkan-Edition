using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private GameObject loadingScreen;

    [SerializeField]
    private Image progressBar;

    private void Awake()
    {
        Instance = this;

    }
    public async void LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        
        loadingScreen.SetActive(true);

        do
        {
            await Task.Delay(200);
            progressBar.fillAmount = scene.progress;

        } while (scene.progress < 0.9f);

        await Task.Delay(2000);

        scene.allowSceneActivation = true;
        loadingScreen.SetActive(false);
    }

    //public class ApiManager : MonoBehaviour
    //{
    //    private string apiUrl = "https://your-api-url/api/users"; // Replace with your actual API endpoint

    //    void Start()
    //    {
    //        StartCoroutine(GetData());
    //    }

    //    IEnumerator GetData()
    //    {
    //        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
    //        yield return request.SendWebRequest();

    //        if (request.result == UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log("Received: " + request.downloadHandler.text);
    //            // Process the received data
    //        }
    //        else
    //        {
    //            Debug.LogError("Error: " + request.error);
    //        }
    //    }

    //    IEnumerator PostData()
    //    {
    //        // Create a user object to send to the API
    //        User userToSend = new User
    //        {
    //            ID = 0, // Assuming the server generates the ID
    //            UserName = "NewUser",
    //            // Add other properties as needed
    //        };

    //        // Convert the user object to JSON
    //        string jsonData = JsonUtility.ToJson(userToSend);

    //        // Set up the request
    //        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
    //        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
    //        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    //        request.downloadHandler = new DownloadHandlerBuffer();
    //        request.SetRequestHeader("Content-Type", "application/json");

    //        // Send the request
    //        yield return request.SendWebRequest();

    //        if (request.result == UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log("Data sent successfully");
    //        }
    //        else
    //        {
    //            Debug.LogError("Error: " + request.error);
    //        }
    //    }
    //}
}