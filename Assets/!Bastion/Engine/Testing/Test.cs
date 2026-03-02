#nullable enable

using System;

namespace Bastion.Testing
{
    /// <summary>
    /// BACKWARDS COMPATABILITY PURPOSES! (test manager is considered no longer supported) Base Class for tests, the main test class inherits from this, helps obscure some things you shouldn't touch.
    /// </summary>
    public class TestBasics
    {
        /// <summary>
        /// Name of the test, unused
        /// </summary>
        protected string? _name;
        /// <summary>
        /// Action the test will perform, gets invoked later
        /// </summary>
        protected Action? action;
        /// <summary>
        /// ID of the test, used for debuging
        /// </summary>
        protected ulong ID;
        /// <summary>
        /// Status of the test, changes as the test progreses, fails, passes etc. etc.
        /// </summary>
        protected TestManager.TestStatus testStatus = TestManager.TestStatus.GenError;
        /// <summary>
        /// the script in which you wrote the test
        /// </summary>
        protected string? originScript;
        /// <summary>
        /// The reason(s) for failing
        /// </summary>
        public string failReason = string.Empty;

        /// <summary>
        /// The constructor for the TestBasics class, no user input needed.
        /// </summary>
        public TestBasics()
        {
            System.Random rand = new System.Random();
            byte[] ulongByteArray = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            rand.NextBytes(ulongByteArray);

            ID = BitConverter.ToUInt64(ulongByteArray);
        }

        private void FailTest()
        {
            testStatus = TestManager.TestStatus.Failed;
        }

        /// <summary>
        /// Changes the origin class, you should avoid touching this function as the test does it by itself automatically as needed.
        /// </summary>
        /// <param name="originScript">Path to the script this test was written in</param>
        protected void SetOriginScript(string originScript)
        {
            this.originScript = originScript;
            _name = originScript;
        }

        /// <summary>
        /// Forces a test to fail, only use if you really know what you're doing.
        /// </summary>
        public void ForceFail()
        {
            if (testStatus == TestManager.TestStatus.Failed)
            {
                TestManager.VerboseLog("Force fail skiped, test has already been deemed failed.");
                return;
            }

            failReason = "\nForced\n\n";
            testStatus = TestManager.TestStatus.Failed;
        }

        /// <summary>
        /// Forces a test to fail, but lets you further explain a reason, only use if you really know what you're doing.
        /// </summary>
        /// <param name="failReason">the reason your test failed</param>
        public void ForceFail(string failReason)
        {
            if (testStatus == TestManager.TestStatus.Failed)
            {
                TestManager.VerboseLog("Force fail skiped, test has already been deemed failed.");
                return;
            }

            this.failReason = "\n" + failReason + "\n\n";
            testStatus = TestManager.TestStatus.Failed;
        }

        /// <summary>
        /// Forces a test to pass, only use if you really know what you're doing.
        /// </summary>
        public void ForcePass()
        {
            testStatus = TestManager.TestStatus.Success;
        }

        /// <summary>
        /// The primary function you will use, this is akin to if (x = y), but it automates everything for you, if x != y, it fails the test, if it does it goes to your next expect statement.
        /// You shouldn't need more than this to be entirely honest, if you can think of something let me know and I'll make a ticket for it. Otherwise the force pass and fail exist for a reason... Don't abuse them though, a force pass on an endless fail isn't a true pass and I will catch you...
        /// </summary>
        /// <typeparam name="T1">Any Type, this is the value you are expecting to be something.</typeparam>
        /// <typeparam name="T2">This is what you expect T1 to be, it should be of the same type or you'll probably explode unity, I dunno haven't tested it yet.</typeparam>
        /// <param name="givenValue">This Value</param>
        /// <param name="expectedValue">To Be</param>
        public void Expect<T1, T2>(T1 givenValue, T2 expectedValue)
        {
            if (testStatus == TestManager.TestStatus.Failed)
            {
                TestManager.VerboseLog("Expect skiped, test has already been deemed failed.");
                return;
            }
            if (testStatus == TestManager.TestStatus.Success)
            {
                TestManager.VerboseLog("Expect skiped, test has already been deemed succesful, Force Passed?.");
                return;
            }

            if (givenValue == null)
            {
                UnityEngine.Debug.LogError("[" + ID + "]Test: " + _name + " | givenValue was found null, exiting Expect function ZT00000");
                return;
            }
            else if (expectedValue == null)
            {
                UnityEngine.Debug.LogError("[" + ID + "]Test: " + _name + " | expectedValue was found null, exiting Expect function ZT00001");
                return;
            }

            try
            {
                if (expectedValue.Equals(givenValue))
                {
                    TestManager.VerboseLog("Expect function was true!");
                    return;
                }
                else
                {
                    FailTest();
                    TestManager.VerboseLog("Expect function has deemed " + "[" + ID + "]" + " Test: " + _name + " as failed");
                    failReason = "\nExpected value: " + expectedValue + "\nValue received: " + givenValue + "\n\n";
                    return;
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }
        }
    }

    /// <summary>
    /// BACKWARDS COMPATABILITY PURPOSES! (test manager is considered no longer supported) A test, used to ensure code works without too much manual testing, good practice to use these. visit zinklof.dev/testing for documentation (maybe)
    /// </summary>
    public class Test : TestBasics
    {
        string? fileName;
        /// <summary>
        /// Time to create a test! this is the Constructor for a test!
        /// </summary>
        /// <param name="fileName"> This should match the name of the CS file you're writing in, and is the workaround for StackFrame being a pain, may not be permanant. </param>
        /// <param name="test"> The action of your test. </param>
        public Test(string fileName, Action test)
        {
            if (fileName == null)
            {
                UnityEngine.Debug.LogError("[" + ID + "]Test: " + fileName + " | Test fileName cannot be null, test did not create successfully ZT00002");
                testStatus = TestManager.TestStatus.GenError;
                return;
            }
            this.fileName = fileName;

            /*string[] res = System.IO.Directory.GetFiles(Application.dataPath, fileName, SearchOption.AllDirectories);
            if (res.Length == 0)
            {
                UnityEngine.Debug.LogError("[" + ID + "]Test: " + fileName + " | Res.length == 0, should not happen, ummmm... ZT00005");
                return;
            }*/
            SetOriginScript(fileName);
            TestManager.VerboseLog(originScript);

            action = test;
        }

        /// <summary>
        /// Run the test!
        /// </summary>
        public void RunTest()
        {
            TestManager.VerboseLog("runtest");
            testStatus = TestManager.TestStatus.Started;
            DateTime startTime = DateTime.Now;

            if (action == null)
            {
                UnityEngine.Debug.LogError("[" + ID + "]Test: " + _name + " | Action was found to be null, invoked before assigning action? ZT00003");
                return;
            }    
            else if (testStatus == TestManager.TestStatus.GenError) 
            {
                UnityEngine.Debug.LogWarning("[" + ID + "]Test: " + _name + " | Test was told to run, however it has a generation error ZT00004");
                return;
            }

            if (originScript == null)
                originScript = "";
            if (fileName == null) 
                fileName = "";

            TestManager.VerboseLog("invoke action");
            action.Invoke();

            TimeSpan timeSpan = DateTime.Now - startTime;
            if (testStatus != TestManager.TestStatus.Failed)
            {
                testStatus = TestManager.TestStatus.Success;
                UnityEngine.Debug.Log("<color=#00ff00>[" + ID + "]" + "Test: " + _name + "\n<size=20><b>PASSED</size></color></b> (" + timeSpan.TotalMilliseconds + "ms)");
                TestManager.LogTest(true, fileName);
            }
            if (testStatus == TestManager.TestStatus.Failed)
            {
                UnityEngine.Debug.Log("<color=#ff0000>[" + ID + "]" + "Test: " + _name + "\n<size=20><b>FAILED</size></color></b> (" + timeSpan.TotalMilliseconds + "ms)" + failReason);
                TestManager.LogTest(false, fileName);
            }
            TestManager.VerboseLog("finished runtest");
        }
    }
}
