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
        // Detect if the user has manually enabled exactly one car in the editor hierarchy.
        // If so, respect that selection (very useful for testing in the editor).
        int activeCarCount = 0;
        int activeCarIndex = -1;
        if (cars != null)
        {
            for (int i = 0; i < cars.Length; i++)
            {
                if (cars[i] != null && cars[i].activeSelf)
                {
                    activeCarCount++;
                    activeCarIndex = i;
                }
            }
        }

        if (activeCarCount == 1)
        {
            index = activeCarIndex;
        }
        else
        {
            index = PlayerPrefs.GetInt("carIndex", 0);
        }

        // Deactivate all cars and cameras first to ensure only the selected one is active
        if (cars != null)
        {
            for (int i = 0; i < cars.Length; i++)
            {
                if (cars[i] != null)
                {
                    cars[i].SetActive(false);
                }
            }
        }
        if (carCameras != null)
        {
            for (int i = 0; i < carCameras.Length; i++)
            {
                if (carCameras[i] != null)
                {
                    carCameras[i].SetActive(false);
                }
            }
        }

        // Activate only the selected car and its camera
        if (cars != null && index >= 0 && index < cars.Length && cars[index] != null)
        {
            cars[index].SetActive(true);
        }
        if (carCameras != null && index >= 0 && index < carCameras.Length && carCameras[index] != null)
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