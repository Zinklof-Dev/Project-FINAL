using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public void LoadScene(int id)
    {
        Application.LoadLevel(id);
    }
}
