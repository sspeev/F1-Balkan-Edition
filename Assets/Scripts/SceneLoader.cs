using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Loads the scene which is given as a string parameter
    /// </summary>
    /// <param name="name"></param>
    public void ChangeScene(string name)
    {
        GameManager.Instance.LoadScene(name);
    }
}
