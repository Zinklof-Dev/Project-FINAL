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

            ApplyQualitySettings();
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
            // it may, in some odd edge case cause a null reference exception here. it shouldn't, as awake should always run before control is returned to the creating function...
            // but it is unity, so who knows, maybe that changes one day, or it just chooses to be silly as unity does.
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

        private static void ApplyQualitySettings()
        {
            QualitySettings.vSyncCount = SettingsManager.FetchSetting("vsync", "0", "QualitySettings", MissingMode.AddThenMinorSave, Engine.verbose);
            QualitySettings.antiAiliasing = SettingsManager.FetchSetting("anti_ailiasing", "4", "QualitySettings", MissingMode.AddThenMinorSave, Engine.verbose);
            QualitySettings.anisotropicFiltering = SettingsManager.FetchSetting("anisotropic_filtering", "16", "QualitySettings", MissingMode.AddThenMinorSave, Engine.verbose);
            QualitySettings.shadowDistance = SettingsManager.FetchSetting("shadow_distance", "512", "QualitySettings", MissingMode.AddThenMinorSave, Engine.verbose);
            QualitySettings.shadowResolution = SettingsManager.FetchSetting("shadow_resolution", "1024", "QualitySettings", MissingMode.AddThenMinorSave, Engine.verbose);
            QualitySettings.textureQualityLevel = SettingsManager.FetchSetting("texture_quality_level", "0", "QualitySettings", MissingMode.AddThenMinorSave, Engine.verbose);
        
            // add logic here to get the enumeration from settings file, don't know the enums int values right now so i'll have to do it when i have intelisense to inform me since unity documentation lacks documentation.

            // be ready for an extra long line
            Screen.SetResolution(SettingsManager.FetchSetting("resolutionx", "1920", "Video", MissingMode.AddThenMinorSave, engine.verbose), SettingsManager.FetchSetting("resolutiony", "1080", "Video", MissingMode.AddThenMinorSave, engine.verbose))

            // unity ignores target framerate if you have vsync on, not exactly sure why since vsync is hardware based, and application.targetframerate is software based, so they very well could both be applied at once if you wanted them to? but hey, whatever
            if (QualitySettings.vSyncCount == 0)
            {
                Application.targetFrameRate = SettingsManager.FetchSetting("framerate", "-1", "Video", MissingMode.AddThenMinorSave, Engine.verbose)
            }
        }
    }
}