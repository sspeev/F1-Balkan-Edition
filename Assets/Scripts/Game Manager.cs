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
}