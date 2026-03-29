using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour
{
    private float timePassed;

    // Update is called once per frame
    void Update()
    {
        if (timePassed > 72)
        {
            SceneManager.LoadScene(1);
        }

        timePassed += Time.deltaTime;
    }
}
