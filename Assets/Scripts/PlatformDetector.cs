using UnityEngine;

public class PlatformDetector : MonoBehaviour
{
    public GameObject mobileUI;
    public GameObject windowsUI;

    void Start()
    {
        // Check if the current platform is Android or iOS
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            mobileUI.SetActive(true);
            windowsUI.SetActive(false);
        }

        else 
        {
            mobileUI.SetActive(false);
            windowsUI.SetActive(true);
        }
    }
}
