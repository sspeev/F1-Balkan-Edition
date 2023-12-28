using System.Collections.Generic;
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

    private Dictionary<string, int> scenes = new()
    {
        {"GameWelcomeScene", 0},
        {"MenuScene", 1},
        { "GameScene", 2 },
        {"TutorialTrack", 3}
    };

    public LoadingScene loader = new();

    public bool BrainControls { get; set; } = false;

    public void LoadGame()
    {
        loader.LoadScene(scenes["GameScene"]);
    }
    public void LoadTutorialTrack()
    {
        SceneManager.LoadScene("TutorialTrack");
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Home()
    {
        if (main.gameObject.activeSelf == true)
        {
            main.gameObject.SetActive(false);
        }
        else
        {
            main.gameObject.SetActive(true);
        }
    }
    public void Options()
    {
        if (main.gameObject.activeSelf == true)
        {
            main.gameObject.SetActive(false);
            options.gameObject.SetActive(true);
        }
        if (brainControls.isOn)
        {
            BrainControls = true;
        }
        else BrainControls = false;
    }
}
