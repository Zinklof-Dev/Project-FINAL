using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour
{
    public float timeTillExit = 72;

    private float timePassed;

    // Update is called once per frame
    void Update()
    {
        if (timePassed > timeTillExit)
        {
            SceneManager.LoadScene(1);
        }

        timePassed += Time.deltaTime;
    }
}
