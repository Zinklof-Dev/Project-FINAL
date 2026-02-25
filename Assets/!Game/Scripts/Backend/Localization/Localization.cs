using System;
using System.IO;
using System.Collections.Generic;

namespace BOTD.Localization {
    internal struct Loc 
    {
        public string key {get; private set;}
        public string value {get; private set;}

        public Loc(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }

    internal struct LocalizationLibrary
    {
        public string libraryName {get; private set;}
        public Loc[] localizations {get; private set;}

        public LocalizationLibrary(string name, List<Localizations> Locs)
        {
            this.libraryName = name;
            this.Localizations = locs.ToArray;
        }
    }

    public static class Internal 
    {
        static private LocalizationLibrary[] LocalizedGame;

        static bool LoadLocalization(string loadLang = "English.loc")
        {
            string currentLib = "none";
            int currentLibIndex = 0;
            List<LocalizationLibrary> libs = new List<LocalizationLibrary>();
            List<Loc> currentLocs = new List<Loc>();

            int currentLine = 0;
            
            try
            {
                using (StreamReader sr = new StreamReader(Application.dataPath + "/JSON/Localization" + loadLang))
                {
                    string line = null;
                    currentline++;

                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] values = line.Split(":");

                        if (values.length > 2)
                        {
                            string fix = "";

                            // starts at one, goes to each string after one and adds it to one, nulls that line in order to save on memory, stops once I is greater than the length of the array
                            for (int i = 1; i <= lines.length; i++;)
                            {
                                fix = fix + lines[i];
                                lines[i] = null;
                            }

                            values[1] = fix;
                        }

                        if (values[1] == null || "")
                        {
                            Debug.LogWarning("Line: " + currentLine + " | Second half of localization line is blank or null?")
                            continue;
                        }
                        
                        if (values[0] = "[lib]")
                        { 
                            //ignore repeats if in a lib already
                            if (values[1] == currentLib)
                            {
                            continue;
                            }
                            
                            if (CheckForRepeats(values[1]))
                            {
                                Debug.LogError("Localization Library already exists, two different structs will be made, this is not performant, memory performant, and will likely result in lookup issues, make sure to define a library once and only once!");
                            }

                            libs.Add(new LocalizationLibrary(values[1], currentLocs));
                            currentLocs = new List<Loc>();
                        }
                        else
                        {
                            currentLocs.Add(new Loc(values[0], values[1]));
                        }
                    }
                    //stream reader finished, wrap up final library
                    libs.Add(new LocalizationLibrary(values[1], currentLocs));
                    currentLocs = null; // clean memory even though garbage will handle it

                    LocalizedGame = libs.ToArray();
                }
            }
            catch
            {
                Debug.LogWarning("Localization fetch error, file may not exist?");
            }
        }

        public static string LookUpKey(string key, string library = "")
        {
            if (library != "" || library != null)
            {
                if (library.ToLower() == "all")
                {
                    foreach (LocalizationLibrary ll in LocalizedGame)
                    {
                        foreach (Loc l in ll.localizations)
                        {
                            if (l.key == key)
                                return l.value;
                        }
                    }
                    return key;
                }

                foreach (LocalizationLibrary ll in LocalizedGame)
                {
                    if (ll.libraryName == library)
                    {
                        foreach (Loc l in ll.localizations)
                        {
                            if (l.key == key)
                                return l.value;
                        }
                    }
                }
                return key;
            }
            else
            {
                Debug.LogWarning("A localization was fetched without a library querry, this is bad practice and costs more compute power/time! if this is the only way, please set the search library to all");
                foreach (LocalizationLibrary ll in LocalizedGame)
                {
                    foreach (Loc l in ll.localizations)
                    {
                        if (l.key == key)
                            return l.value;
                    }
                }
                return key;
            }
            return key;
        }
    }
}