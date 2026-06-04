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
        for (int i = 0; i < cars.Length; i++)
        {
            if (cars[i] != null)
            {
                cars[i].SetActive(i == index);
            }
        }
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        if (next != null)
        {
            next.interactable = (index < cars.Length - 1);
        }
        if (prev != null)
        {
            prev.interactable = (index > 0);
        }
    }

    public void Next()
    {
        if (index < cars.Length - 1)
        {
            index++;
            for (int i = 0; i < cars.Length; i++)
            {
                if (cars[i] != null)
                {
                    cars[i].SetActive(i == index);
                }
            }
            UpdateButtons();
        }
    }

    public void Prev()
    {
        if (index > 0)
        {
            index--;
            for (int i = 0; i < cars.Length; i++)
            {
                if (cars[i] != null)
                {
                    cars[i].SetActive(i == index);
                }
            }
            UpdateButtons();
        }
    }

    public void Race()
    {
        SceneManager.LoadSceneAsync("GameScene");
        PlayerPrefs.SetInt("carIndex", index);
        PlayerPrefs.Save();
    }
}
