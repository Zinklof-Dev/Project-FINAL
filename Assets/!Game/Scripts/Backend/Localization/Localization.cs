using System;

namespace BOTD.Localization {
    internal struct Loc 
    {
        public string key {get; private set;}
        public string value;
    }

    internal struct LocalizationLibrary
    {
        public string libraryName {get; private set;}
        public Loc[] localizations {get; private set;}
    }

    public static class Internal 
    {
        private LocalizationLibrary[] LocalizedGame;

        static bool LoadLocalization()
        {

        }

        public static string LookUpKey(string key, string library = "")
        {
            if (library != "" || library != null)
            {
                foreach (LocalizationLibrary ll in LocalizedGame)
                {
                    if (ll.name == library)
                    {
                        foreach (Loc l in ll.localizations)
                        {
                            if (l.key = key)
                                return l.value;
                        }
                    }
                }
                return key;
            }
            else
            {
            }
        }
    }
}