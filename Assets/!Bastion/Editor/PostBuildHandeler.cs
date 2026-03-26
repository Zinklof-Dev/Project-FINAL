using System;
using System.Collections.Generics;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Bastion.Editor
{
    public class PostBuildHandeler
    {
        [PostProcessBuildAttribute(100)]
        public static void incrementBuild()
        {
            // need to parse the settings in order to be able to check the build number, theoretically i could bake this into a seperate file thats binary, but for now this is the most basic and logical solution until more stuff needs to be accessed in this kind of way
            Bastion.SettingsManager.ParseSettings();

            int value = Bastion.SettingsManager.FetchSetting("engine.build_number", "0", "Engine", MissingMode.AddThenMinorSave);

            value++;

            // change the value of the build number setting in memory
            Bastion.SettingsManager.ChangeSetting("engine.build_number", value.ToString());

            // save all the settings in memory
            Bastion.SettingsManager.SaveSettings();
        }


        // sorry never nesters, but i'm not making a function just for the sw, you can deal with some nesting in the background of your post build pipeline :P
        [PostProcessBuildAttribute(101)]
        public static void MoveSettingsToBuildLocation(BuildTarget target, string pathToBuild)
        {
            //Bastion.SettingsManager.SaveSettings();

            try
            {
                using (StreamReader sr = new StreamReader(Application.DataPath + "/settings.bcfg"))
                {
                    string line;

                    while ((line = sr.readLine()) != null)
                    {
                        using (StreamWriter sw = new StreamWriter(pathToBuild + "/settings.bcfg"))
                        {
                            sw.writeLine(line);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Exception when transfering settings file to build location, exception lowered to warning as this isn't a build destroying issue and you can move it yourself.");
                Debug.LogWarning(e.Message + e.StackTrace);
            }
        }
    }
}