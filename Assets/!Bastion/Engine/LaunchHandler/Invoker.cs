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
            Bastion.Engine.localizationFallbackEnglish = SettingsManager.FetchSetting("engine.allow_localization_fallback_to_english", "true", "Engine", MissingMode.AddThenMinorSave, Engine.verbose);
            Bastion.Engine.showFPS = SettingsManager.FetchSetting("engine.show_fps", "false", "Engine", MissingMode.AddThenMinorSave, Engine.verbose);
            Bastion.Engine.showOtherPerformance = SettingsManager.FetchSetting("engine.show_more_performance", "false", "Engine", MissingMode.AddThenMinorSave, Engine.verbose);

            //Bastion.SettingsManager.ParseSettings(Engine.verbose);

            string language = Bastion.SettingsManager.FetchSetting("language", "English", "Gameplay", MissingMode.AddThenMinorSave, Engine.verbose);

            Bastion.Localization.ParseLocalization(language, Engine.verbose);

            Bastion.ConsoleV2.Assembler.Initialize(Engine.verbose);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void PostInitialize()
        {
            // create the bmm if it doesn't exist, this is really only for editor where people may be entering into the game environment from a scnene other than start
            if (BastionMonoManager.instance == null)
            {
                Debug.LogWarning($"{Bastion.Branding.engineLogPrefix}[Invoker.PostInitialize()] BastionMonoManager is null on first scene load, creating one now, may result in unwanted variable assignments and undoubtably null reference exceptions in build! <size=11>(Ensure you create one in your first/intro scene so the rest of the game has one)</size>");
                GameObject bmm =  new GameObject("BastionMonoManager", typeof(BastionMonoManager));
            }

            // if the above code has to run to create one (for editor purposes is the only reason the above code exists, always have one in your intro scene so it doesn't have to)
            // it may, in some odd edge case cause a null reference exception here. it shouldn't awak should always run before control is returned to the creating function...
            // but it is unity, so who knows, maybe that changes one day, or it just chooses to not do it in the right timing.
            BastionMonoManager.instance.Initialize();
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