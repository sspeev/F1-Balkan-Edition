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

    [SerializeField]
    private string api;

    private void Awake()
    {
        Instance = this;
        StartCoroutine(GetData(api));
        StartCoroutine(PostData(api));
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
    private IEnumerator GetData(string api)
    {
        UnityWebRequest request = UnityWebRequest.Get(api);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received: " + request.downloadHandler.text);
            // Process the received data
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    private IEnumerator PostData(string api)
    {
        // Create a user object to send to the API
        UserDTO userToSend = new();
        var currentTime = new LapTimer();

        // Convert the user object to JSON
        string jsonData = JsonUtility.ToJson(userToSend);

        // Set up the request
        UnityWebRequest request = new UnityWebRequest(api, "POST");
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