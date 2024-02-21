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
        if (cars.Length > index && index > -1)
        {
            cars[index].SetActive(true);
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