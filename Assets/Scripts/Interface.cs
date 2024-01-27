using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Here are implemented the OnClick action on the buttons in the main menu
/// </summary>
public class Interface : MonoBehaviour
{
    [SerializeField]
    private Canvas options;

    [SerializeField]
    private Canvas main;

    [SerializeField]
    private Canvas leaderboard;

    [SerializeField]
    private Toggle brainControls;

    public bool BrainControls { get; set; } = false;

    public void Home()
    {
        if (main.gameObject.activeSelf == false)
        {
            main.gameObject.SetActive(true);
            options.gameObject.SetActive(false);
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
        else
        {
            BrainControls = false;
        }
    }

    public void Leaderboard()
    {
        if (leaderboard.gameObject.activeSelf == false)
        {
            leaderboard.gameObject.SetActive(true);
            main.gameObject.SetActive(false);
        }
        else
        {
            leaderboard.gameObject.SetActive(false);
            main.gameObject.SetActive(true);
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
}
