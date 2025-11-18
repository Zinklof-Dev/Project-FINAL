using UnityEngine;

public class MMButtonUtils : MonoBehaviour
{
    [SerializeField] GameObject[] containers;

    public void ExitToDesktop()
    {
        Application.Quit();
    }

    public void CloseAllContainers()
    {
        foreach (GameObject container in containers)
        {
            container.SetActive(false);
        }
    }

    public void OpenContainer(string name)
    {
        foreach (GameObject container in containers)
        {
            if (gameObject.name == name)
            {
                container.SetActive(true);
                break;
            }
        }

        Debug.LogWarning("No container named: " + name + " found.");
    }
}
