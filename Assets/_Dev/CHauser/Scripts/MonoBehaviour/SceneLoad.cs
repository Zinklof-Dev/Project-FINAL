using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void StartNewGame()
    {
        DataPersistenceManager.newGameOnStart = true;
        LoadScene("Camp_Scene");
    }

    public void StartLoadedGame()
    {
        DataPersistenceManager.loadGameOnStart = true;
        LoadScene("Camp_Scene");
    }
}
