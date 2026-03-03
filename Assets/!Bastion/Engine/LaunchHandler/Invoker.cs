using UnityEngine;

namespace Bastion.LaunchHandler
{
    public static class Invoker
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnInitialize()
        {
            Bastion.SettingsManager.ParseSettings(true);

            string language = Bastion.SettingsManager.FetchSetting("language", "English", "Gameplay", MissingMode.AddThenMinorSave, true);

            Bastion.Localization.ParseLocalization(language);

            Bastion.ConsoleV2.Assembler.Initialize();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void PostInitialize()
        {
            if (BastionMonoManager.instance == null)
            {
                Debug.LogWarning($"{Bastion.Branding.engineLogPrefix}[Invoker.PostInitialize()] BastionMonoManager is null on first scene load, creating one now, may result in unwanted variable assignments!");
                GameObject bmm =  new GameObject("BastionMonoManager", typeof(BastionMonoManager));
            }
            //BastionMonoManager.instance.InstantiateConsole();
        }

        /// <summary>
        /// Wrapper around the private OnInitialize function. Not recommended to use unless debuging or hitting a vital error, will likely be fairly expensive to run.
        /// </summary>
        public static void ReInitialize()
        {
            OnInitialize();
            PostInitialize();
        }
    }
}