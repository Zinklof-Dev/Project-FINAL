using System.IO;
using System.Collections.Generic;
using UnityEngine;
using ZinklofDev.LocalizationOLD;
using System;

namespace ZinklofDev
{
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

    public static class Langs
    {
        public static string en {get; private set;}
        public static string it {get; private set;}
        public static string fr {get; private set;}
        public static string de {get; private set;}
        public static string es {get; private set;}
        public static string gb {get; private set;}
        public static string lc {get; private set;}

        static Langs()
        {
            en = "English";
            it = "Italian";
            fr = "Français";
            de = "Deutsch";
            es = "Español";
            gb = "Goblin";
            lc = "Lolcat";
        }
    }

    public static class Localization
    {
        // static array that contains all the info we need, stays here as private as we querry it and send only the absolute neccesary info out.
        private static LocalizationLibrary[] LocalizedGame;

        private static List<UncompiledLocLibrary> uncompiledLibraries = new List<UncompiledLocLibrary>();

        public static void CompileLocalization(string lang = "English", bool verbose = false)
        {
            // make a list of the uncompiled libraries (like the library struct but list instead of array so we can add to it easier till we've finished searching all files)

            // current library being worked on so we don't have to constantly fetch the list
            UncompiledLocLibrary workingLibrary = new UncompiledLocLibrary("none");

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

                            Debug.Log(values[0] + " | " + values[1]);

                            // check if library def and isn't of the one we are current working on
                            if (values[0] == "[lib]" && values[1].ToLower() != workingLibrary.libraryName)
                            {
                                workingLibrary = SwitchLibrary(values[1].ToLower(), workingLibrary);
                                continue;
                            }

                            // now we can assume we have a loc line
                            if (values[1][0].ToString() == " ")
                            {
                                char[] fix = values[1].ToCharArray();
                                if (fix[0] == ' ')
                                    fix[0] = '\0';

                                values[1] = new string(fix);
                            }

                            if (values[0][values[0].Length - 1].ToString() == " ")
                            {
                                char[] fix = values[0].ToCharArray();
                                if (fix[fix.Length-1] == ' ')
                                    fix[fix.Length - 1] = '\0';

                                values[0] = new string(fix);
                            }
                            
                            workingLibrary.Add(values[0], values[1]);
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
                uncompiledLibraries.Clear();

                if (verbose)
                {
                    foreach (LocalizationLibrary ll in LocalizedGame)
                    {
                            Debug.Log(ll.ToString());
                    }
                }
            }
            catch(Exception e)
            {
                // uh oh
                Debug.LogWarning("Localization ran into an exception that it has no way of handling!");
                Debug.LogException(e);
            }
        }

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

        public static string ColorEditorLine(string line)
        {
            string regColor = "#77dddd";
            string libColor = "#dede65";
            string keyColor = "#deae45";
            string commentColor = "#757575";
            string colonColor = "#bbbbbb";
            string errorColor = "#de2525";
             
            int commentStart = line.IndexOf("#");

            bool libLine = line.Contains("[lib]");
            bool commentLine = (line[0] == '#');
            bool keyLine = line.Contains(":");

            line = $"<color={regColor}>" + line + "</color>"; // set base color

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
                line.Replace("[lib]", $"<color={libColor}>[lib]</color>");
                line.Replace(":", $"<color={colonColor}>:</color>");

                if (line.IndexOf("#") != -1 && libLine)
                {
                    string comment = line.Substring(line.IndexOf("#"));
                    line.Remove(line.IndexOf("#"));
                    line += $"</color><color={commentColor}>{comment}";
                }
            }

            return line;
        }

        private static string SearchSpecificLibForKey(string key, string library, bool verbose = false)
        {
            foreach (LocalizationLibrary ll in LocalizedGame)
            {
                if (ll.libraryName == library)
                {
                    foreach (Loc l in ll.localizations)
                    {
                        if (l.key == key)
                            return l.value;
                    }
                    if (verbose)
                        Debug.LogWarning("When Searching " + ll.libraryName + " (" + library + ") for the key " + key + " nothing was found, returning key." );
                    return key;
                }
            }
            if (verbose)
                Debug.LogWarning("When searching for " + library + " no library was found, returning key.");
            return key;
        }

        private static string SearchAllLibsForKey(string key, bool passWarning = false, bool verbose = false)
        {
            if (!passWarning)
                Debug.LogWarning("You are looking up a key without a known library, this is less performant than if you know the library due to searching more. If this is your only option please set the library querry to 'all'");

            foreach (LocalizationLibrary ll in LocalizedGame)
            {
                foreach (Loc l in ll.localizations)
                {
                    if (l.key == key)
                        return l.value;
                    else
                        continue;
                }
            }
            if (verbose)
                Debug.LogWarning("when searching ALL libraries for " + key + " nothing was found, returning key.");
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
                Debug.Log(allFiles.Length + " Files loading\n" + files);
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