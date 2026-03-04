using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Bastion.ConsoleV2;

namespace Bastion
{
    internal struct SettingLibrary
    {
        public string name;
        public List<Setting> containedSettings;

        public SettingLibrary(string name)
        {
            this.name = name;
            containedSettings = new List<Setting>();
        }
    }

    /// <summary>
    /// represents a setting.
    /// </summary>
    public struct Setting
    {
        public bool empty;
        public string key;
        public string value;
        public string library;

        public Setting(string key, string value)
        {
            this.key = key;
            this.value = value;
            library = "misc";
            empty = false;
        }
        
        public Setting(string key, string value, string library)
        {
            this.key = key;
            this.value = value;
            this.library = library;
            empty = false;
        }
        
        /// <summary>
        /// Constructs a fully empty setting, mostly used for edge case purposes.
        /// </summary>
        /// <returns>Empty Setting</returns>
        public static Setting Empty()
        {
            Setting var =  new Setting();
            var.SetEmpty();
            return var;
        }

        /// <summary>
        /// Sets the setting to empty, kind of like nullifying it.
        /// </summary>
        public void SetEmpty()
        {
            key = string.Empty;
            value = string.Empty;
            library = string.Empty;
            empty = true;
        }

        public void Setlibrary(string library)
        {
            this.library = library;
        }

        public override string ToString()
        {
            return value;
        }

        public static implicit operator string(Setting setting)
        {
            return setting.value;
        }

        public static implicit operator int(Setting setting)
        {
            if (int.TryParse(setting.value, out int value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning(setting.key + " Tried to convert to int but failed, possible bad input in the file or bad cast, returning -1");
                return -1;
            }
        }

        public static implicit operator float(Setting setting) 
        { 
            if (float.TryParse(setting.value, out float value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning(setting.key + " Tried to convert to float but failed, possible bad input in the file or bad cast, returning -1f");
                return -1f;
            }
        }

        public static implicit operator bool(Setting setting)
        {
            if (bool.TryParse(setting.value,out bool value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning(setting.key + " Tried to convert to boolean but failed, possible bad input in the file or bad cast, returning false");
                return false;
            }
        }
    }

    /// <summary>
    /// How the settings manager should handle not finding the setting you're searching for.
    /// </summary>
    public enum MissingMode
    { 
        /// <summary>
        /// Return an empty setting and pretend nothing happened
        /// </summary>
        Ignore = 0,
        /// <summary>
        /// Add the setting to memory to be saved at a later date.
        /// </summary>
        Add = 1,
        /// <summary>
        /// Add the setting to memory, then run a full save operation, sorting the file properly.
        /// </summary>
        AddThenSave = 2,
        /// <summary>
        /// Add the setting to memory, then add it and only it to the end of the file, ensuring that it at least gets into the settings file without potentially saving other unsaved settings when saving should not occur.
        /// </summary>
        AddThenMinorSave = 3,
        /// <summary>
        /// Return the default value, but don't add to memory or save the value. functionally keeping it hidden from the player if they never specify it in the .bcfg
        /// </summary>
        ReturnDefault = 4,
    }

    public static class SettingsManager
    {
        public static string filePath = "";
        public static string fileName = "settings";
        public static string fileExtension = ".bcfg";

        private static Setting[] settings = new Setting[1];

        public static Setting FetchSetting(string key, string defaultValue = "", string defaultLibrary = "misc", MissingMode missingMode = MissingMode.Add, bool verbose = false)
        {
            string logPrefix = $"{Branding.engineLogPrefix}[SettingsManager.FetchSetting()] ";

            foreach (Setting s in settings)
            {
                if (s.key == key)
                {
                    if (verbose)
                        Debug.Log(logPrefix + $"setting found for key {key}, returning setting object with value of {s.value}");

                    return s;
                }
            }

            // we can now assume the setting doesn't exist.
            // did I change the list name to avoid CS0128? yes, yes i did, sue me.
            switch (missingMode)
            {
                case MissingMode.Ignore:
                    if (verbose)
                        Debug.Log(logPrefix + "Setting not found, ignoring request");
                    return Setting.Empty();

                case MissingMode.Add:
                    if (verbose)
                        Debug.Log(logPrefix + "Setting not found, adding to memory but not saving to file");
                    List<Setting> list = new List<Setting>(settings)
                    {
                        new Setting(key, defaultValue, defaultLibrary)
                    };
                    settings = list.ToArray();
                    return settings[settings.Length - 1];

                case MissingMode.AddThenSave:
                    if (verbose)
                        Debug.Log(logPrefix + "Setting not found, adding to memory then saving all settings to file");
                    List<Setting> list1 = new List<Setting>(settings)
                    {
                        new Setting(key, defaultValue, defaultLibrary)
                    };
                    settings = list1.ToArray();
                    SaveSettings();
                    return settings[settings.Length - 1];

                case MissingMode.AddThenMinorSave:
                    if (verbose)
                        Debug.Log(logPrefix + "Setting not found, adding to memory then appending to settings file");
                    List<Setting> list2 = new List<Setting>(settings)
                    {
                        new Setting(key, defaultValue, defaultLibrary)
                    };
                    settings = list2.ToArray();
                    SaveSetting(settings[settings.Length-1]);
                    return settings[settings.Length - 1];

                case MissingMode.ReturnDefault:
                    if (verbose)
                        Debug.Log(logPrefix + "Setting not found, returning default value but not saving to memory or file");
                    return new Setting(key, defaultValue, defaultLibrary);
            };

            if (verbose)
                Debug.Log(logPrefix + "left switch case without a result... uh oh");

            // unreachable in practice, only here so the compiler stops bitching about CS0161
            return Setting.Empty();
        }

        /// <summary>
        /// Call at start of runtime, wipes current memory of settings and parses again, can be expensive.
        /// </summary>
        /// <param name="verbose">Whether or not to spew out tons of verbose debug information, not recommended.</param>
        /// <returns>Boolean representing if it (probably) failed or succeded.</returns>
        [Command("Wipes memory of settings then parses the settings file")]
        public static bool ParseSettings(bool verbose = false)
        {
            string fullpath = Application.dataPath + "/" + filePath + fileName + fileExtension;

            string currentLibrary = "misc";

            string logPrefix = $"{Branding.engineLogPrefix}[SettingsManager.ParseSettings()] ";

            List<Setting> settingsList = new List<Setting>();

            int totalSettingsParsed = 0;
            int totalLibrariesParsed = 1; // account for misc

            try
            {
                using (StreamReader sr = new StreamReader(fullpath))
                {
                    string line = "";

                    while ((line = sr.ReadLine()) != null)
                    {
                        // skip blank lines
                        if (line.Length == 0)
                            continue;
                        else if (line[0] == '\0' || line[0] == '\n')
                            continue;

                        // skip comment lines
                        if (line.Replace(" ", "")[0] == '#' || line.Replace(" ", "")[0] == '/')
                            continue;
                        
                        //check if library
                        if (line[0] == '[')
                        {
                            //currentLibrary = line.Replace("[", "").Replace("]", ""); // yk... i could just save the substring between the two brackets, that'd be cheaper...

                            currentLibrary = line.Substring(1, line.Length-2); // I did just that lol

                            if (verbose)
                                Debug.Log(logPrefix + "changed library to " + currentLibrary);

                            totalLibrariesParsed++;
                            continue;
                        }

                        // split into key, and value
                        string[] values = SplitLine(line, verbose);

                        if (verbose)
                            Debug.Log(logPrefix + values[0] + " | " + values[1]);

                        settingsList.Add(new Setting(values[0], values[1], currentLibrary));

                        totalSettingsParsed++;
                    }
                }

                settings = settingsList.ToArray();

                if (verbose)
                    Debug.Log(logPrefix + $"{totalSettingsParsed} settings were parsed across {totalLibrariesParsed} libraries");

                return true;
            }
            catch (Exception e)
            {
                // uh oh
                Debug.LogWarning(logPrefix + "ran into an exception that it has no way of handling!\n" + e.Message + "\n" + e.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Saves the current settings in memory to the settings file, overwriting the file.
        /// </summary>
        /// <param name="verbose">Whether or not to spew out tons of verbose debug information, not recommended.</param>
        /// <returns>Boolean representing if it (probably) failed or succeded.</returns>
        [Command("Saves the current settings in memory to the settings file, overwriting the file.")]
        public static bool SaveSettings(bool verbose = false)
        {
            // define the full path to the settings file
            string fullpath = Application.dataPath + "/" + filePath + fileName + fileExtension;

            // create the list of libraries and use the CompileAllLibraries function to create a list of library structs containing all our settings
            List<SettingLibrary> libraries = CompileAllLibraries();

            string logPrefix = $"{Branding.engineLogPrefix}[SettingsManager.SaveSettings()] ";

            // if this by some magic returns a null class, log an error and return to avoid exceptions, then freak the fuck out
            if (libraries == null)
            {
                Debug.LogError(logPrefix + "Compiling libraries resulted in a null list (how the fuck?)");
                return false;
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(fullpath))
                {
                    // per library in the list we made earlier
                    foreach (SettingLibrary sl in libraries)
                    {
                        // write its name so when being read later we can tell what library a setting belongs to (mostly for file readability purposes)
                        sw.WriteLine($"[{sl.name}]");

                        // per setting in that library
                        foreach (Setting s in sl.containedSettings)
                        {
                            // write that setting down!
                            sw.WriteLine($"{s.key} : {s.value}");
                        }
                    }
                }

                // neat we did it, return true
                return true;
            }
            catch(Exception e)
            {
                // uh oh
                Debug.LogWarning(logPrefix + "ran into an exception that it has no way of handling!\n" + e.Message + "\n" + e.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Force saves a specific setting by appending it and its libary to the current setting file.
        /// </summary>
        /// <param name="setting">The seting to force save.</param>
        /// <param name="verbose">Whether or not to spew out tons of verbose debug information, not recommended.</param>
        /// <returns>Boolean representing if it (probably) failed or succeded.</returns>
        public static bool SaveSetting(Setting setting, bool verbose = false)
        {
            // define the full path to the settings file
            string fullpath = Application.dataPath + "/" + filePath + fileName + fileExtension;

            string logPrefix = $"{Branding.engineLogPrefix}[SettingsManager.SaveSetting()] ";

            try
            { 
                using (StreamWriter sw = new StreamWriter(fullpath, true))
                {
                    // write the library, then the setting so its somewhat neat, and on read later we know what library it belongs to if we read then save, at which point it'd get sorted properly.
                    sw.WriteLine($"[{setting.library}]");
                    sw.WriteLine($"{setting.key} : {setting.value}");
                }
            }
            catch(Exception e)
            {
                // uh oh
                Debug.LogWarning(logPrefix + "ran into an exception that it has no way of handling!\n" + e.Message + "\n" + e.StackTrace);
                return false;
            }

            // yay we did it return true!
            return true;
        }

        // this could be better... but eh, later me cry problem.
        private static List<SettingLibrary> CompileAllLibraries()
        {
            List<string> libraryStrings = new List<string>();
            List<SettingLibrary> libraries = new List<SettingLibrary>();

            foreach (Setting s in settings)
            {
                if (libraryStrings.Contains(s.library))
                    continue;
                else
                    libraryStrings.Add(s.library);
            }

            foreach (string str in libraryStrings)
            {
                SettingLibrary sl = new SettingLibrary(str);

                foreach (Setting s in settings)
                {
                    if (s.library == str)
                        sl.containedSettings.Add(s);
                }

                libraries.Add(sl);
            }

            return libraries;
        }


        // modified version of the SplitLine from the localization system.
        // removed the [lib] check, added the ' ' remover that happens at the end of the localization handler that frankly could've been in this function
        private static string[] SplitLine(string line, bool verbose = false)
        {
            string logPrefix = $"{Bastion.Branding.engineLogPrefix}[SettingsManager.SplitString()] ";

            string[] split = new string[2];

            int index = 0;

            index = line.IndexOf(":");

            // add each char from first half, ignoring the index
            for (int i = 0; i < index; i++)
            {
                split[0] += line[i];
            }

            // add each char from second half, ignoring index
            for (int i = index + 1; i < line.Length; i++)
            {
                split[1] += line[i];
            }

            if (verbose)
            {
                Debug.Log(logPrefix + $"index = {index}");
                Debug.Log(logPrefix + $"split[0] = {split[0]}");
                Debug.Log(logPrefix + $"split[1] = {split[1]}");
            }

            // force key to lowercase in order to avoid case sensitivty on the keys
            split[0] = split[0].ToLower();

            if (split[0][0] == '[' && split[1].IndexOf("#") != -1)
            {
                split[1].Remove(split[1].IndexOf("#"));
            }

            if (split[1][0] == ' ')
                split[1] = split[1].Remove(0, 1);

            if ((split[0][split[0].Length - 1]) == ' ')
                split[0] = split[0].Remove(split[0].Length - 1, 1);

            return split;
        }
    }
}
