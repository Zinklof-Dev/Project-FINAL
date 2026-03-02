using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bastion.Utils.Assemblies
{
    /// <summary>
    /// Class of various methods that should help in finding assemblies
    /// </summary>
    public static class FindAssembly
    {
        /// <summary>
        /// Searches for and finds an assembly by the provided name using linq
        /// </summary>
        /// <param name="name">Name of the assmebly you want to find</param>
        /// <returns>Assembly</returns>
        public static Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().
                   SingleOrDefault(assembly => assembly.GetName().Name == name);
        }
    }


    // credit some random guy on stack overflow a few years back, odd that i didn't give credit in the first place as i usually do. -zink

    /// <summary>
    ///     Intent: Get referenced assemblies, either recursively or flat. Not thread safe, if running in a multi
    ///     threaded environment must use locks.
    /// </summary>
    public static class GetReferencedAssemblies
    {
        static void Demo()
        {
            var referencedAssemblies = Assembly.GetEntryAssembly().MyGetReferencedAssembliesRecursive();
            var missingAssemblies = Assembly.GetEntryAssembly().MyGetMissingAssembliesRecursive();
            // Can use this within a class.
            //var referencedAssemblies = this.MyGetReferencedAssembliesRecursive();
        }

        public class MissingAssembly
        {
            public MissingAssembly(string missingAssemblyName, string missingAssemblyNameParent)
            {
                MissingAssemblyName = missingAssemblyName;
                MissingAssemblyNameParent = missingAssemblyNameParent;
            }

            public string MissingAssemblyName { get; set; }
            public string MissingAssemblyNameParent { get; set; }
        }

        private static Dictionary<string, Assembly> _dependentAssemblyList;
        private static List<MissingAssembly> _missingAssemblyList;

        /// <summary>
        ///     Intent: Get assemblies referenced by entry assembly. Not recursive.
        /// </summary>
        public static List<string> MyGetReferencedAssembliesFlat(this Type type)
        {
            var results = type.Assembly.GetReferencedAssemblies();
            return results.Select(o => o.FullName).OrderBy(o => o).ToList();
        }

        /// <summary>
        ///     Intent: Get assemblies currently dependent on entry assembly. Recursive.
        /// </summary>
        public static Dictionary<string, Assembly> MyGetReferencedAssembliesRecursive(this Assembly assembly)
        {
            _dependentAssemblyList = new Dictionary<string, Assembly>();
            _missingAssemblyList = new List<MissingAssembly>();

            InternalGetDependentAssembliesRecursive(assembly);

            // Only include assemblies that we wrote ourselves (ignore ones from GAC).
            var keysToRemove = _dependentAssemblyList.Values.Where(
                o => o.GlobalAssemblyCache == true).ToList();

            foreach (var k in keysToRemove)
            {
                _dependentAssemblyList.Remove(k.FullName.MyToName());
            }

            return _dependentAssemblyList;
        }

        /// <summary>
        ///     Intent: Get missing assemblies.
        /// </summary>
        public static List<MissingAssembly> MyGetMissingAssembliesRecursive(this Assembly assembly)
        {
            _dependentAssemblyList = new Dictionary<string, Assembly>();
            _missingAssemblyList = new List<MissingAssembly>();
            InternalGetDependentAssembliesRecursive(assembly);

            return _missingAssemblyList;
        }

        /// <summary>
        ///     Intent: Internal recursive class to get all dependent assemblies, and all dependent assemblies of
        ///     dependent assemblies, etc.
        /// </summary>
        private static void InternalGetDependentAssembliesRecursive(Assembly assembly)
        {
            // Load assemblies with newest versions first. Omitting the ordering results in false positives on
            // _missingAssemblyList.
            var referencedAssemblies = assembly.GetReferencedAssemblies()
                .OrderByDescending(o => o.Version);

            foreach (var r in referencedAssemblies)
            {
                if (String.IsNullOrEmpty(assembly.FullName))
                {
                    continue;
                }

                if (_dependentAssemblyList.ContainsKey(r.FullName.MyToName()) == false)
                {
                    try
                    {
                        var a = Assembly.ReflectionOnlyLoad(r.FullName);
                        _dependentAssemblyList[a.FullName.MyToName()] = a;
                        InternalGetDependentAssembliesRecursive(a);
                    }
                    catch (Exception ex)
                    {
                        _missingAssemblyList.Add(new MissingAssembly(r.FullName.Split(',')[0], assembly.FullName.MyToName()));

                        UnityEngine.Debug.LogWarning(ex.Message + "\n" + ex.StackTrace); // only thing i added iirc -zink
                    }
                }
            }
        }

        private static string MyToName(this string fullName)
        {
            return fullName.Split(',')[0];
        }
    }
}
