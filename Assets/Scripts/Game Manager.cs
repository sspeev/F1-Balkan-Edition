using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private GameObject loadingScreen;

    [SerializeField]
    private Image progressBar;

    private int index;

    public GameObject[] cars;
    public GameObject[] carCameras;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        index = PlayerPrefs.GetInt("carIndex");

        // Deactivate all cars and cameras first to ensure only the selected one is active
        for (int i = 0; i < cars.Length; i++)
        {
            if (cars[i] != null)
            {
                cars[i].SetActive(false);
            }
        }
        for (int i = 0; i < carCameras.Length; i++)
        {
            if (carCameras[i] != null)
            {
                carCameras[i].SetActive(false);
            }
        }

        // Activate only the selected car and its camera
        if (index >= 0 && index < cars.Length && cars[index] != null)
        {
            cars[index].SetActive(true);
        }
        if (index >= 0 && index < carCameras.Length && carCameras[index] != null)
        {
            carCameras[index].SetActive(true);
        }
        index = 0;
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

        await Task.Delay(500);

        scene.allowSceneActivation = true;
        loadingScreen.SetActive(false);
    }
}