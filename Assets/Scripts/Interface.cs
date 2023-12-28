using UnityEngine;
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
        else BrainControls = false;
    }
    public void Exit()
    {
        Application.Quit();
    }
}
