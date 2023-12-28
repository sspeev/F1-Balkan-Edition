using System.Collections;

public interface ISceneLoader
{
    void LoadScene(int sceneId);

    IEnumerator LoadSceneAsync(int sceneId);
}