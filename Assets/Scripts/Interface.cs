using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interface : MonoBehaviour
{
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
}
