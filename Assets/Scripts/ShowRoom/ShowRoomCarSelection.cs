using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowRoomCarSelection : MonoBehaviour
{
    public GameObject[] cars;
    public Button next;
    public Button prev;
    int index;

    void Start()
    {
        index = 0;
        cars[0].SetActive(true);
        cars[1].SetActive(false);
        cars[2].SetActive(false);
        cars[3].SetActive(false);
    }
    void Update()
    {
        if(index == 3)
        {
            next.interactable = false;
        }
        else
        {
            next.interactable = true;
        }
        if(index == 0)
        {
            prev.interactable = false;
        }
        else
        {
            prev.interactable = true;
        }
    }

    public void Next()
    {
        index ++;

        for(int i = 0; i < cars.Length;i++)
        {
            cars[i].SetActive(false);
            cars[index].SetActive(true);
        }

        PlayerPrefs.SetInt("carIndex", index);
        PlayerPrefs.Save();
    }

    public void Prev()
    {
        index --;

        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].SetActive(false);
            cars[index].SetActive(true);
        }

        PlayerPrefs.SetInt("carIndex", index);
        PlayerPrefs.Save();
    }

    public void Race()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
}
