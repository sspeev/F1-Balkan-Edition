using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowRoomCarSelection : MonoBehaviour
{
    public GameObject[] cars;
    public Button next;
    public Button prev;
    int index = 0;

    void Start()
    {
        index = 0;
        cars[0].SetActive(true);
        cars[1].SetActive(false);
        cars[2].SetActive(false);
        cars[3].SetActive(false);
        cars[4].SetActive(false);
    }
    void Update()
    {
        if (index == 4)
        {
            next.interactable = false;
        }
        else
        {
            next.interactable = true;
        }
        if (index == 0)
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
        index++;

        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].SetActive(false);
            cars[index].SetActive(true);
        }
    }

    public void Prev()
    {
        index--;

        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].SetActive(false);
            cars[index].SetActive(true);
        }
    }
    public void Race()
    {
        SceneManager.LoadSceneAsync("GameScene");
        PlayerPrefs.SetInt("carIndex", index);
        PlayerPrefs.Save();
    }
}
