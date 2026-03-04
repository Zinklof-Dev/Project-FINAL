using UnityEngine;
using Bastion.ConsoleV2;

namespace Bastion.LaunchHandler
{
    public static class Invoker
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnInitialize()
        {
            Bastion.SettingsManager.ParseSettings(false);

            Bastion.Engine.verbose = SettingsManager.FetchSetting("engine.verbose", "false", "Engine", MissingMode.AddThenMinorSave, false);

            Bastion.SettingsManager.ParseSettings(Engine.verbose);

            string language = Bastion.SettingsManager.FetchSetting("language", "English", "Gameplay", MissingMode.AddThenMinorSave, Engine.verbose);

            Bastion.Localization.ParseLocalization(language, Engine.verbose);

            Bastion.ConsoleV2.Assembler.Initialize(Engine.verbose);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void PostInitialize()
        {
            bool makeConsole = SettingsManager.FetchSetting("engine.console", "true", "Engine", MissingMode.ReturnDefault, Engine.verbose);

            if (BastionMonoManager.instance == null)
            {
                Debug.LogWarning($"{Bastion.Branding.engineLogPrefix}[Invoker.PostInitialize()] BastionMonoManager is null on first scene load, creating one now, may result in unwanted variable assignments and undoubtably null reference errors in build!");
                GameObject bmm =  new GameObject("BastionMonoManager", typeof(BastionMonoManager));
            }

            if (makeConsole)
                BastionMonoManager.instance.InstantiateConsole();
        }

        /// <summary>
        /// Wrapper around the private OnInitialize function. Not recommended to use unless debuging or hitting a vital error, will likely be fairly expensive to run.
        /// </summary>
        [Command("Re-does the initialization functions", false, true)]
        public static void ReInitialize()
        {
            OnInitialize();
            PostInitialize();
        }
    }
}