using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField]
    private Canvas options;

    [SerializeField]
    private Canvas main;

    [SerializeField]
    private Toggle brainControls;

    public bool BrainControls { get; set; } = false;

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void LoadTutorialTrack()
    {
        SceneManager.LoadScene("TutorialTrack");
    }
    public void Welcome()
    {
        SceneManager.LoadScene("ShowRoom");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Options()
    {
        if (main.gameObject.activeSelf == true)
        {
            main.gameObject.SetActive(false);
            options.gameObject.SetActive(true);
        }
        else
        {
            main.gameObject.SetActive(true);
            options.gameObject.SetActive(false);
        }
        if (brainControls.isOn)
        {
            BrainControls = true;
        }
        else BrainControls = false;
    }
}
