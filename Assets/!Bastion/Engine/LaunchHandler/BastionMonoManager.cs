using UnityEngine;

namespace Bastion.LaunchHandler
{
    public class BastionMonoManager : MonoBehaviour
    {
        public static BastionMonoManager instance;

        public GameObject consolePrefab;
        public GameObject bastionCanvasPrefab;

        private void Awake()
        {
            if (instance != null && instance != null)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        public void InstantiateConsole()
        {
            if (Bastion.ConsoleV2.Console.console != null)
            {
                Destroy(Bastion.ConsoleV2.Console.console);
            }
            
            GameObject canvas = Instantiate(bastionCanvasPrefab);

            GameObject console = Instantiate(consolePrefab);

            console.transform.SetParent(canvas.transform, false);

            DontDestroyOnLoad(canvas);

            Bastion.Engine.canvas = canvas;

            Bastion.ConsoleV2.Console.console = console;
        }
    }
}