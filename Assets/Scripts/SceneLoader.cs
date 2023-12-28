using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        GameManager.Instance.LoadScene(name);
    }
}
