using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using Bastion.ConsoleV2;

namespace Bastion
{
    internal struct Loc
    {
        public string key { get; private set; }
        public string value { get; private set; }

        public Loc(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }

    internal struct LocalizationLibrary
    {
        public string libraryName { get; private set; }
        public Loc[] localizations { get; private set; }

        public LocalizationLibrary(string name, List<Loc> locs)
        {
            this.libraryName = name;
            this.localizations = locs.ToArray();
        }

        public override string ToString()
        {
            string returnVar = libraryName + "\n";

            foreach (Loc loc in localizations)
            {
                returnVar += (loc.key + " : " + loc.value + "\n");
            }

            return returnVar;
        }
    }

    internal struct UncompiledLocLibrary
    {
        public string libraryName;
        public List<Loc> locs;

        public UncompiledLocLibrary(string name)
        {
            this.libraryName = name;
            this.locs = new List<Loc>();
        }

        public LocalizationLibrary Compile()
        {
            return new LocalizationLibrary(libraryName, locs);
        }

        public void Add(string key, string value) // wrapper in order to add to the list with less characters becasuse it looks nicer :P
        {
            locs.Add(new Loc(key, value));
        }
    }

    /// <summary>
    /// A class containing standard langauge spellings in accordance to their standard abreviation, so you don't have to remember how to spell Français, just that its Langs.fr!
    /// </summary>
    public static class Langs
    {
        // --- Official European Union Languages ---
        public static string bg { get; private set; } // Bulgarian
        public static string hr { get; private set; } // Croatian
        public static string cs { get; private set; } // Czech
        public static string da { get; private set; } // Danish
        public static string nl { get; private set; } // Dutch
        public static string en { get; private set; } // English
        public static string et { get; private set; } // Estonian
        public static string fi { get; private set; } // Finnish
        public static string fr { get; private set; } // French
        public static string de { get; private set; } // German
        public static string el { get; private set; } // Greek
        public static string hu { get; private set; } // Hungarian
        public static string ga { get; private set; } // Irish
        public static string it { get; private set; } // Italian
        public static string lv { get; private set; } // Latvian
        public static string lt { get; private set; } // Lithuanian
        public static string mt { get; private set; } // Maltese
        public static string pl { get; private set; } // Polish
        public static string pt { get; private set; } // Portuguese
        public static string ro { get; private set; } // Romanian
        public static string sk { get; private set; } // Slovak
        public static string sl { get; private set; } // Slovenian
        public static string es { get; private set; } // Spanish
        public static string sv { get; private set; } // Swedish
    
        // --- Major Non-EU European Languages ---
        public static string ru { get; private set; } // Russian
        public static string uk { get; private set; } // Ukrainian
        public static string tr { get; private set; } // Turkish
    
        // --- Top Spoken Non-European Languages (2025 Rankings) ---
        public static string zh { get; private set; } // Mandarin Chinese
        public static string hi { get; private set; } // Hindi
        public static string ar { get; private set; } // Arabic (Standard)
        public static string bn { get; private set; } // Bengali
        public static string id { get; private set; } // Indonesian
        public static string ur { get; private set; } // Urdu
        public static string ja { get; private set; } // Japanese
        public static string mr { get; private set; } // Marathi
        public static string te { get; private set; } // Telugu
        public static string vi { get; private set; } // Vietnamese
        public static string ko { get; private set; } // Korean

        // --- Some Joke Languages ---
        public static string gb { get; private set; } // goblin, based on goblinbane
        public static string lc { get; private set; } // lolcat
        public static string ps { get; private set; } // pirate speak
        public static string dg { get; private set; } // doge
        public static string bf { get; private set; } // brainfuck (the programming langauge)
        public static string iv { get; private set; } // Intellectually Distinct village folk

        static Langs()
        {
            // European Union
            bg = "Български"; hr = "Hrvatski"; cs = "Čeština"; da = "Dansk";
            nl = "Nederlands"; en = "English"; et = "Eesti"; fi = "Suomi";
            fr = "Français"; de = "Deutsch"; el = "Ελληνικά"; hu = "Magyar";
            ga = "Gaeilge"; it = "Italiano"; lv = "Latviešu"; lt = "Lietuvių";
            mt = "Malti"; pl = "Polski"; pt = "Português"; ro = "Română";
            sk = "Slovenčina"; sl = "Slovenščina"; es = "Español"; sv = "Svenska";

            // Non-EU European
            ru = "Русский"; uk = "Українська"; tr = "Türkçe";

            // Global Non-European
            zh = "普通话"; hi = "हिन्दी"; ar = "العربية"; bn = "বাংলা";
            id = "Bahasa Indonesia"; ur = "اُردُو"; ja = "日本語"; mr = "मराठी";
            te = "తెలుగు"; vi = "Tiếng Việt"; ko = "한국어";

            // Joke Langauges
            gb = "Goblin"; lc = "LolCat"; ps = "Pirate Speak"; dg = "Doge";
            bf = "Brainfuck"; iv = "Intellectually Distinct Village Folk";
        }
    }

    public static class Localization
    {
        // static array that contains all the info we need, stays here as private as we querry it and send only the absolute neccesary info out.
        private static LocalizationLibrary[] LocalizedGame;

        private static List<UncompiledLocLibrary> uncompiledLibraries = new List<UncompiledLocLibrary>();

        private static int totalLibrairesParsed;

        /// <summary>
        /// Call at start of runtime, or when langauge setting changes. wipes current memory of localization and parses again. Can be expensive.
        /// </summary>
        /// <param name="lang">Language to use, see Bastion.Lang class for references to standard language spellings via their abrevation.</param>
        /// <param name="verbose">Whether or not to spew out tons of verbose debug information, not recommended.</param>
        [Command("Wipes memory of localization and parses again")]
        public static bool ParseLocalization(string lang = "English", bool verbose = false)
        {
            // make a list of the uncompiled libraries (like the library struct but list instead of array so we can add to it easier till we've finished searching all files)

            // current library being worked on so we don't have to constantly fetch the list
            UncompiledLocLibrary workingLibrary = new UncompiledLocLibrary("none");

            int totalLocsParsed = 0;

            string logPrefix = $"{Branding.engineLogPrefix}[Localization.ParseLocalization()] ";

            try
            {
                // fetch all files
                string[] allFiles = FetchFileList(verbose, lang);

                // work on every file
                foreach (string file in allFiles)
                {
                    // open streamreader for this file
                    using (StreamReader sr = new StreamReader(file))
                    {
                        // set library to none to start if it isn't.
                        if (workingLibrary.libraryName != "none")
                            SwitchLibrary("none", workingLibrary);

                        string line = null;

                        // itterate through each line
                        while((line = sr.ReadLine()) != null)
                        {
                            // skip blank lines
                            if (line.Length == 0)
                                continue;
                            else if (line[0] == '\0' || line[0] == '\n')
                                continue;

                            // skip comment lines
                            if (line.Replace(" ", "")[0] == '#' || line.Replace(" ", "")[0] == '/')
                                continue;

                            // split into key, and value
                            string[] values = SplitLine(line);

                            if (verbose)
                            Debug.Log(logPrefix + values[0] + " | " + values[1]);

                            // check if library def
                            if (values[0] == "[lib]")
                            {
                                // if it isn't the same as the one we are wokring on right now, swich over.
                                if (values[1].ToLower() != workingLibrary.libraryName)
                                {
                                    workingLibrary = SwitchLibrary(values[1].ToLower(), workingLibrary);
                                }

                                // continue in order to not try and parse it like a loc line
                                continue;
                            }

                            // now we can assume we have a loc line
                            if (values[1][0] == ' ')
                                values[1] = values[1].Remove(0, 1);

                            //Debug.Log(values[0][values[0].Length - 1]);
                            //Debug.Log(values[0][values[0].Length - 1]);

                            if ((values[0][values[0].Length - 1]) == ' ')
                                 values[0] = values[0].Remove(values[0].Length - 1, 1);
                            
                            workingLibrary.Add(values[0], values[1]);

                            totalLocsParsed++;
                        }
                    }
                }

                //We have gathered and parsed every file, now we need to compile each uncompiled libary into an array based one for performance and memory.
                List<LocalizationLibrary> HalfCompiled = new List<LocalizationLibrary>();
                
                foreach (UncompiledLocLibrary ull in uncompiledLibraries)
                {
                    LocalizationLibrary ll = ull.Compile();
                    HalfCompiled.Add(ll);
                }

                LocalizedGame = HalfCompiled.ToArray();
                uncompiledLibraries.Clear(); // immedietly free some memory

                if (verbose)
                {
                    foreach (LocalizationLibrary ll in LocalizedGame)
                    {
                            Debug.Log(logPrefix + ll.ToString());
                    }

                    Debug.Log(logPrefix + $"{totalLocsParsed} Localizations were parsed across {LocalizedGame.Length} libraries");
                }

                return true;
            }
            catch(Exception e)
            {
                // uh oh
                Debug.LogWarning(logPrefix + " ran into an exception that it has no way of handling!\n" + e.Message + "\n" + e.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Search the currently loaded Localization for the value of a key. (recommended you define the second variable for more performant searches)
        /// </summary>
        /// <param name="key">The key to search with.</param>
        /// <param name="library">The Library to search.</param>
        /// <param name="verbose">Whether or not to spew out tons of verbose debug information, not recommended.</param>
        /// <returns>The value of the key passed in within acordance of the last compiled langauges .loc files</returns>
        // wrapper to combine the usage of two functions into one.
        public static string LookUpKey(string key, string library = "", bool verbose = false)
        {
            library = library.ToLower();
            key = key.ToLower();

            if (library == null || library == "")
            {
                return SearchAllLibsForKey(key, false, verbose);
            }
            else if (library == "all")
            {
                return SearchAllLibsForKey(key, true, verbose);
            }
            else
            {
                return SearchSpecificLibForKey(key, library, verbose);
            }
        }

        /// <summary>
        /// Not for normal use; used by the Localization Editor.
        /// </summary>
        /// <param name="line">The Line to colorize</param>
        /// <returns>line but colorful, because why not? (to make how it would be compiled more "visible")</returns>
        static internal string ColorEditorLine(string line)
        {
            string regColor = "#77dddd";
            string libColor = "#efef45";
            string keyColor = "#deae45";
            string commentColor = "#757575";
            string colonColor = "#dddddd";
            string errorColor = "#ff0000";
             
            int commentStart = line.IndexOf("#");

            if (line == "")
            {
                return line;
            }

            bool libLine = line.Contains("[lib]");
            bool commentLine = (line[0] == '#');
            bool keyLine = line.Contains(":");

            int index = 0;

            if (commentLine)
            {
                line = $"<color={commentColor}>{line}</color>";
            }
            else if (!libLine && !keyLine)
            {
                line = $"<color={errorColor}>{line}</color>";
            }
            else
            {
                if (line.IndexOf("#") != -1 && libLine)
                {
                    string comment = line.Substring(line.IndexOf("#"));
                    line.Remove(line.IndexOf("#"));
                    line += $"</color><color={commentColor}>{comment}";
                }

                if (keyLine)
                {
                    index = line.IndexOf(":");
                    line = $"<color={keyColor}>" + line.Substring(0, index-1) + "</color> " + line.Substring(index);
                }

                line = line.Replace("[lib]", $"<color={libColor}>[lib]</color>");
                line = line.Replace(":", $"<color={colonColor}>:</color>");

                line = $"<color={regColor}>" + line + "</color>"; // set base color
            }

            return line;
        }

        private static string SearchSpecificLibForKey(string key, string library, bool verbose = false)
        {
            string logPrefix = $"{Branding.engineLogPrefix}[Localization.SearchSpecificLibForkey()] ";

            foreach (LocalizationLibrary ll in LocalizedGame)
            {
                if (ll.libraryName == library)
                {
                    foreach (Loc l in ll.localizations)
                    {
                        if(verbose)
                        {
                            string LogVal = l.key + " | ";

                            foreach (char c in l.key)
                            {
                                LogVal += ((byte)c) + " ";
                            }

                            Debug.Log(logPrefix + LogVal);
                        }

                        if (l.key == key)
                        {
                            Debug.Log(logPrefix + $"Found {l.value} when loking for {l.key}, returning.");
                            return l.value;
                        }
                        else
                            continue;
                    }
                    if (verbose)
                        Debug.LogWarning(logPrefix + "When Searching " + ll.libraryName + " (" + library + ") for the key " + key + " nothing was found, returning key." );
                    return key;
                }
            }
            if (verbose)
                Debug.LogWarning(logPrefix + "When searching for " + library + " no library was found, returning key.");
            return key;
        }

        private static string SearchAllLibsForKey(string key, bool passWarning = false, bool verbose = false)
        {
            string logPrefix = $"{Branding.engineLogPrefix}[Localization.SearchAllLibsForKey()] ";

            if (!passWarning)
                Debug.LogWarning(logPrefix + "You are looking up a key without a known library, this is less performant than if you know the library due to searching more. If this is your only option please set the library querry to 'all'");

            foreach (LocalizationLibrary ll in LocalizedGame)
            {
                foreach (Loc l in ll.localizations)
                {
                    if (verbose)
                    {
                        string LogVal = l.key + " | ";

                        foreach (char c in l.key)
                        {
                            LogVal += ((byte)c) + " ";
                        }

                        Debug.Log(logPrefix + LogVal);
                    }

                    if (l.key == key)
                    {
                        Debug.Log(logPrefix + $"Found {l.value} when loking for {l.key}, returning.");
                        return l.value;
                    }
                    else
                        continue;
                }
            }
            if (verbose)
                Debug.LogWarning(logPrefix + "When searching ALL libraries for " + key + " nothing was found, returning key.");
            return key;
        }

        private static string[] SplitLine(string line)
        {
            string[] split = new string[2];

            int index = 0;

            if (line.ToLower().Contains("[lib]"))
            {
                split[0] = "[lib]";

                split[1] = line.Replace("[lib]", "").Replace(" ", "");

                index = split[1].IndexOf("#");

                if (index != -1) // remove comments
                {
                    split[1].Remove(split[1].IndexOf("#"));
                }

                return split;
            }

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

            // force key to lowercase in order to avoid case sensitivty on the keys
            split[0] = split[0].ToLower();

            if (split[0] == "[lib]" && split[1].IndexOf("#") != -1)
            {
                split[1].Remove(split[1].IndexOf("#"));
            }

            return split;
        }

        private static string[] FetchFileList(bool verbose, string lang)
        {
            string logPrefix = $"{Branding.engineLogPrefix}[Localization.FetchFileList()] ";

            // get the file location of every file within the language folder and its subfolders
            string[] allFiles = Directory.GetFiles(Application.dataPath + "/JSON/Localization/" + lang, "*.loc", SearchOption.AllDirectories);

            // verbose print all files found
            if (verbose)
            {
                string files = "";
                foreach (string s in allFiles)
                {
                    files += s + "\n";
                }
                Debug.Log(logPrefix + allFiles.Length + " Files loading\n" + files);
            }

            return allFiles;
        }

        private static UncompiledLocLibrary SwitchLibrary(string name, UncompiledLocLibrary workingLibrary)
        {
            bool savedWorkingLibrary = false;
            UncompiledLocLibrary ?returnLibrary = null;

            // begin to check if the library needing saved exists, and if the library we are looking for exists
            for (int index = 0; index < uncompiledLibraries.Count; index++)
            {
                // if we find the one needing saved, save it
                if (uncompiledLibraries[index].libraryName == workingLibrary.libraryName)
                {
                    uncompiledLibraries[index] = workingLibrary;
                    savedWorkingLibrary = true;
                }
                // if we find the one being searched for, and we already saved the one needing save it, just return it right away.
                if (uncompiledLibraries[index].libraryName == name && savedWorkingLibrary)
                    return uncompiledLibraries[index];
                // otherwise save it to the return variable to return later and keep looking
                else if (uncompiledLibraries[index].libraryName == name)
                    returnLibrary = uncompiledLibraries[index];
            }
            // if we never saved the library (aka it didn't exist prior), then add it to the list.
            if (!savedWorkingLibrary)
                uncompiledLibraries.Add(workingLibrary);

            // if we never found the library we were trying to fetch, then make a new one of the same name and return that.
            returnLibrary ??= new UncompiledLocLibrary(name);

            return (UncompiledLocLibrary)returnLibrary;
        }
    }
}
