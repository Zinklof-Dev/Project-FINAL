using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using Bastion.Utils.Fun;
using System.Reflection;

namespace Bastion.Testing
{
    /// <summary>
    /// BACKWARDS COMPATABILITY PURPOSES! (test manager is considered no longer supported) This class manages everything in the backend, from logging, handling settings, verbose logging, etc. etc. Runs after awake runs on every script in your FIRST scene
    /// </summary>
    public static class TestManager
    {
        // looking back... NONE OF THESE SEEM TO CLEAR THEMSELVES SO THEY SAT IN MEMORY FOREVER AFTER YOU RAN TESTS... EW! -zink

        static DateTime start;
        static DateTime end;
        static List<ulong> usedIDs = new List<ulong>();
        static List<TestedClass> originScripts = new List<TestedClass>();
        static List<Test> tests = new List<Test>();
        //static List<string> log = new List<string>();
        /// <summary>
        /// Verbose is a bool that toggles whether or not the TestManager spits endless information, you must change this in an awake function in your FIRST loaded scene so that it is changed before tests get run.
        /// </summary>
        public static bool verbose = false;
        /// <summary>
        /// The enum for test statuses.
        /// </summary>
        public enum TestStatus
        {
            /// <summary>
            /// The Test ran into an error when being generated.
            /// </summary>
            GenError,
            /// <summary>
            /// The test has been created, but not run.
            /// </summary>
            Created,
            /// <summary>
            /// The test is running.
            /// </summary>
            Started,
            /// <summary>
            /// The test has failed.
            /// </summary>
            Failed,
            /// <summary>
            /// The test passed.
            /// </summary>
            Success,
        }
        private static ulong _totalTests;
        private static ulong _totalPasses;
        private static ulong _totalFailures;
        private static double _totalPercentage;

        private static TimeSpan _totalTimetaken;

        /// <summary>
        /// The total tests in your project.
        /// </summary>
        public static ulong totalTests { get { return _totalTests; } }
        /// <summary>
        /// Total passes.
        /// </summary>
        public static ulong totalPasses { get { return _totalPasses; } }
        /// <summary>
        /// Total fails.
        /// </summary>
        public static ulong totalFailures { get { return _totalFailures; } }
        /// <summary>
        /// percentage of tests passed.
        /// </summary>
        public static double totalPercentage { get { return _totalPercentage; } }

        public static void OnFirstSceneLoaded()
        {
            TestManager.VerboseLog("running OnFirstSceneLoaded");

            Test newTest = new Test("Fallback test created", () =>
            {
                Debug.LogError("Tried to run an empty test, error with TestManager?");
            });

            /*FieldInfo[] fields = baseAssembly.GetTypes()
                      .SelectMany(t => t.GetFields())
                      .Where(m => m.GetCustomAttributes(typeof(UnitTest), false).Length > 0)
                      .ToArray();

            Test[] foundTests = baseAssembly.GetTypes()
                .SelectMany (t => t.GetFields())
                .Where(m => m.GetCustomAttributes(typeof (UnitTest), false).Length > 0)
                .Select(s => (Test)s.GetValue(null)).ToArray();*/

            List<Test> foundTests = new List<Test>();

            /*foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                VerboseLog("checking assembly " + a.FullName);
                foreach (Type t in a.GetTypes())
                {
                    //VerboseLog("checking type of " + t.FullName + " in assembly " + a.FullName);
                    foreach (FieldInfo f in t.GetFields())
                    {
                        //VerboseLog("checking field of " + f.Name + " in type " + t.FullName + "in assembly " + a.FullName);
                        if (f.GetType() == typeof(Test))
                        {
                            foundTests.Add((Test)f.GetValue(null));
                            VerboseLog("found a type and added it to the list!");
                        }
                    }
                }
            }*/

            Assembly baseAssembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "Assembly-CSharp");

            foreach (Type t in baseAssembly.GetTypes())
            {
                VerboseLog("Checking type of name: " + t.FullName);
                foreach (FieldInfo f in t.GetFields())
                {
                    if (f.FieldType == typeof(Test))
                    {
                        VerboseLog("Found a fieldInfo of type Test");
                        foundTests.Add((Test)f.GetValue(null));
                    }
                    else
                    {
                        VerboseLog("rejected a fieldInfo for not being of type Test");
                    }
                }
            }

            foreach (Test test in foundTests)
            {
                try
                {
                    VerboseLog("trying to add a test to the use these tests test list!");
                    tests.Add(test);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);

                    VerboseLog("failed to add it to the use these tests list, adding forced failed test");
                    tests.Add(newTest);
                }
            }

            VerboseLog("starting tests");
            start = DateTime.Now;
            foreach (Test test in tests)
            {
                test.RunTest();
            }
            end = DateTime.Now;
            VerboseLog("finished all tests");

            _totalTimetaken = end - start;

            PrintResults();
        }

        /// <summary>
        /// Logs a test finishing, not for you to use, its for the test system only, NO TOUCHY.
        /// </summary>
        /// <param name="passed">Did the test pass?</param>
        /// <param name="fileName">Where did the test come from?</param>
        internal static void LogTest(bool passed, string fileName)
        {
            VerboseLog("logging new test");
             _totalTests = _totalTests + 1;

            if (passed)
            {
                VerboseLog("test was logged as a pass");
                _totalPasses = _totalPasses + 1;
                //log.Add("PASS " + origin);
                //log.Add("  Description:");
            }
            else
            {
                VerboseLog("test was logged as a fail");
                _totalFailures = _totalFailures + 1;
                //log.Add("FAIL " + origin);
                //log.Add("  Description:");
            }

            _totalPercentage = _totalPasses / _totalTests * 100;

            foreach (TestedClass tc in originScripts)
            {
                if (tc.name == fileName)
                {
                    VerboseLog("found a matching testedclass");
                    tc.LogaTest(passed);
                    VerboseLog("after LogaTest");
                    //log.Add("    " + tc.description);
                    return;
                }
            }

            TestedClass tc2 = new TestedClass();
            VerboseLog("registering new testedclass");
            tc2.name = fileName;
            tc2.description = fileName;
            //log.Add("    " + tc2.description);
            tc2.LogaTest(passed);

            originScripts.Add(tc2);
        }

        private struct TestedClass
        {
            public string name;
            public string description;
            public ulong totalTests;
            public ulong passes;
            public ulong failures;
            public double percentage;

            public void LogaTest(bool passed)
            {
                VerboseLog("TestedClass logged function");
                totalTests = totalTests + 1;
                VerboseLog("" + totalTests);

                if(passed)
                {
                    VerboseLog("TestedClass logged function as passed");
                    passes = passes + 1;
                }
                else
                {
                    VerboseLog("TestedClass logged function as failed");
                    failures = failures + 1;
                }

                percentage = passes / totalTests * 100;
            }
        }

        /// <summary>
        /// Logs a testing Verbose Log
        /// </summary>
        /// <param name="message">Message</param>
        public static void VerboseLog(string message)
        {
            if (verbose)
            {
                Debug.Log("[VERBOSE TEST LOGGING]" + message);
            }
        }
        /// <summary>
        /// Logs a testing Verbose Error Log
        /// </summary>
        /// <param name="message">Message</param>
        public static void VerboseLogError(string message)
        {
            if (verbose)
            {
                Debug.LogError("[VERBOSE TEST LOGGING]" + message);
            }
        }
        /// <summary>
        /// Logs a testing Verbose Warning Log
        /// </summary>
        /// <param name="message">Message</param>
        public static void VerboseLogWarning(string message)
        {
            if (verbose)
            {
                Debug.LogWarning("[VERBOSE TEST LOGGING]" + message);
            }
        }

        private static void PrintResults()
        {
            VerboseLog("printing results...");

            string gradeValue = Grading.ZinkScale(_totalPercentage);

            if (totalTests < 0)
            {
                return;
            }

            try
            {
                string path = Application.dataPath + @"\TestLogs\TestResults" + start.ToString("yyyyy-MM-dd--HH-mm-ss-fff") + ".txt";

                StreamWriter testLogFile = File.CreateText(path);

                //FileStream testLogFile = File.Open(path, FileMode.Append, FileAccess.Write);
                string[] lines = {start.ToString("MM/dd/yyyyy gg | HH:mm:ss:FFF") + " || TESTS RESULTS - ZINKLOF.DEV Unity C# Test System", " ", " ", "ALL RESULTS:", " " };
                foreach (string line in lines)
                {
                    testLogFile.WriteLine(line);
                }

                /*foreach (string s in log)
                {
                    testLogFile.Write(s);
                }*/

                foreach (TestedClass tc in originScripts)
                {
                    VerboseLog("" + tc.totalTests);
                    string line = "    " + tc.name + "    Total Tests: " + tc.totalTests + "   |   " + tc.passes + " Passed " + tc.failures + " Failed. " + tc.percentage + "% " + Grading.ZinkScale(tc.percentage) + "\n";
                    testLogFile.Write(line);
                }

                string[] finalLines = { "\n", "    Total Tests: " + _totalTests + "   |   " + _totalPasses + " Passed " + _totalFailures + " Failed.", "\n\n Grade: ", totalPasses / totalTests * 100 + "% " + Grading.ZinkScale(totalPasses / totalTests * 100), "\n Time Elapsed " + _totalTimetaken.TotalMilliseconds + "ms", "\n END LOG || " + end.ToString("MM/dd/yyyyy gg | HH:mm:ss:FFF") };

                foreach (string line in finalLines)
                {
                    testLogFile.Write(line);
                }

                testLogFile.Close();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogException(ex);
            }
            VerboseLog("finished printing results");
        }
    }
}
