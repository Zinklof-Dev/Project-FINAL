using UnityEngine;
using UnityEngine.SceneManagement;


// Temporary win / loss screen manager, will move to its' own script, I just don't want .meta file conflicts to occur again.
// Will definitly depricate in the future in favor of a more complex level fail / succeed script, but this will work for now for Experience Pinellas.
// Also def gonna comment this out so that the team doesn't have to do the compiler error shuffle tomorrow.
// Also gonna include these comments in the new script that I'm going to migrate to tommorow. 

public class TempWinLossManagerEP : MonoBehaviour // EP stands for Experience Pinellas.
{
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject lossScreen;

    public void Win()
    {
        winScreen.SetActive(true);
    }

    public void Loss()
    {
        lossScreen.SetActive(true);
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}