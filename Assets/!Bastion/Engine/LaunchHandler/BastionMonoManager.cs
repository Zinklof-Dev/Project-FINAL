using UnityEngine;

namespace Bastion.LaunchHandler
{
    public class BastionMonoManager : MonoBehaviour
    {
        public static BastionMonoManager instance;

        public GameObject consolePrefab;
        public GameObject bastionCanvasPrefab;
        public GameObject performanceVisualizerPrefab;

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


        // changed the below from instantiateConsole to initialize due to the addition of handling other components.

        /// <summary>
        /// For use by the Invoker class that the engine runs on start, not recommended for your use unless bugfixing.
        /// </summary>
        public void Initialize()
        {
            // if the bastion canvas exists already destroy it so we don't cause problems in the event we need to reinitialize
            if (Bastion.Engine.canvas != null)
            {
                Destroy(Bastion.Engine.canvas);
            }
            
            // create the canvas
            Bastion.Engine.canvas = Instantiate(bastionCanvasPrefab);

            // create the console
            if ((bool)SettingsManager.FetchSetting("engine.console", "true", "Engine", MissingMode.AddThenMinorSave, Engine.verbose) == true)
                Bastion.ConsoleV2.Console.console = Instantiate(consolePrefab);

            // create the performance visualizer (I was gonna make this make one from scratch but i dont wanna deal with the math of setting it into the right spot with the right scaling so...)
            GameObject pv = Instantiate(performanceVisualizerPrefab);

            // attach the console and pv as a child to the canvas
            ConsoleV2.Console.console.transform.SetParent(Engine.canvas.transform, false);
            pv.transform.SetParent(Engine.canvas.transform, false);

            // set the canvas to DNDOL
            DontDestroyOnLoad(Bastion.Engine.canvas);
        }
    }
}