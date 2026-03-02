using UnityEngine;

namespace Bastion.ConsoleV2
{
    public class InputHandeler : MonoBehaviour
    {
        [Header("TMP References")]
        [SerializeField] private TMPro.TextMeshProUGUI log;
        [SerializeField] private TMPro.TextMeshProUGUI possibleCommandsGUI;
        [SerializeField] private TMPro.TMP_InputField inputField;
        [Header("Console Anim")]
        float targetYOpen = 270;
        float targetYClosed = 822;
        float lerp = 0.06f;
        [Header("Rect Transform References")]
        [SerializeField] RectTransform console;
        [SerializeField] RectTransform logContainer;
        [Header("Log Anim")]
        float maxLogContainerY = 0;
        float minLogContainerY = -9999;
        float logLerp = 0.5f;
        float changeAmmount = -35000;
        bool lockCursor = false;

        private float targetLogContainerY;

        private string[] lastInputs = new string[50];
        private int currentIndex;

        private string[] possibleCommands;
        private string tabfill;

        private void Start()
        {
            Assembler.Initialize();

            Application.logMessageReceived += HandleUnityLog;
            Console.OnNewLog += UpdateLog;

            string knownProblems = "Currently known problems in this version:\n" +
                "Wrong Variables cause exceptions that sometimes kill the Shell\n" +
                "Missformating of () causes exceptions that sometimes kill the shell\n" +
                "<s><color=#9bff9b>GPU Driver may crash on particularly severe exceptions due to an issue on unity's end with texture math</color></s> (One time issue?)\n" +
                "<s><color=#9bff9b>Help command doesn't exist</color></s>\n" +
                "<s><color=#9bff9b>Cheat flag doesn't actually do anything</color></s>\n" +
                "Suggested commands <s><color=#9bff9b>don't tab auto fill, and</color></s> freak out once you start entering variables\n" +
                "Console may spazz to the corner, zero clue why\n" +
                "<s><color=#9bff9b>Log container doesn't scroll, code is in place, variables are not set yet.</color></s>\n" +
                "Console causes slowdown on start, increases exponentially with the ammount of assemblies, classes, and methods you have, this is an issue with using reflection, and can only be worked around in the future." + "\n\n" +

                "<size=25><b>September 5th 2025</b></size>\n" +
                "New Update removes a nullable marker on a string, may explode, I forgot why it was originally nullable so I made it non-nullable without checking.\n\n" +

                "<size=25><b>March 1st 2026 3:45 A.M.</b></size>\n" +
                "New Update: Cheat flag now does something, cheats are off by default, shell.cheats allows you to toggle them on.\n" +
                "Developmental flag added, assembler checks if its not in editor or a debug build, and wont assemble commands that are flagged developmental if it isn't\n" +
                "Log container can now scroll quote on quote forever\n" +
                "console open/close speed slowed down\n" +
                "input handler variables hidden from inspector. no reason to show them.";

            Console.Log("Welcome to Bastion Console V2! You are currently using " + Console.ReleaseType + " " + Console.ReleaseVersion + ", There are " + Console.CommandsRegistered + " commands registered in your project.", "Console", "0fffff", true, 22);
            Console.Log(knownProblems, "", "ff9b9b", true, 15);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tilde) || Input.GetKeyDown(KeyCode.BackQuote))
            {
                Console.isOpen = !Console.isOpen;

                if (Console.isOpen)
                {
                    Cursor.lockState = CursorLockMode.None;
                    inputField.ActivateInputField();
                }
                else
                {
                    if (lockCursor)
                        Cursor.lockState = CursorLockMode.Locked;

                    inputField.DeactivateInputField();
                    inputField.text = "";
                    possibleCommandsGUI.text = "";
                }
            }

            if (Console.isOpen)
            {
                console.localPosition = Vector3.Lerp(console.localPosition, new Vector3(0, targetYOpen, 0), lerp);
                float y = Input.GetAxis("Mouse ScrollWheel") * changeAmmount * Time.deltaTime;
                targetLogContainerY = Mathf.Clamp(targetLogContainerY + y, minLogContainerY, maxLogContainerY);
                logContainer.localPosition = Vector3.Lerp(logContainer.localPosition, new Vector3(0, targetLogContainerY, 0), logLerp);

                if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
                {
                    AppendToLastInputs();
                    Shell.CallCommand(inputField.text);
                    inputField.text = "";
                    inputField.ActivateInputField();
                    currentIndex = -1;
                }

                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    inputField.text = tabfill;
                    inputField.MoveTextEnd(false);
                    inputField.text += ")";
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (lastInputs[currentIndex + 1] == "" || lastInputs[currentIndex + 1] == string.Empty || lastInputs[currentIndex + 1] == null)
                    {
                        return;
                    }

                    currentIndex++;
                    if (currentIndex > 49)
                    {
                        currentIndex = 49;
                    }

                    inputField.text = lastInputs[currentIndex];
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    currentIndex--;
                    if (currentIndex < -1)
                    {
                        currentIndex = -1;
                    }

                    if (currentIndex != -1)
                        inputField.text = lastInputs[currentIndex];
                    else
                        inputField.text = "";
                }
            }
            else
                console.localPosition = Vector3.Lerp(console.localPosition, new Vector3(0, targetYClosed, 0), lerp);
        }

        public void OnInputChanged()
        {
            if (inputField.text == "")
            {
                possibleCommandsGUI.text = "";
                return;
            }

            string[] results = Shell.SearchForCommands(inputField.text);
            possibleCommands = results;

            if (results.Length > 0)
            {
                tabfill = results[0].Split("(")[0];
                tabfill += "(";
            }

            if (results.Length == 0)
            {
                possibleCommandsGUI.text = "";
                return;
            }

            for (int i = 0; i < results.Length; i++)
            {
                string partOne = "";
                string partTwo = "";
                string partThree = "";

                int index = results[i].ToLower().IndexOf(inputField.text.ToLower());

                if (index != -1)
                {
                    partTwo = "<color=#ffffff>" + results[i].Substring(index, inputField.text.Length) + "</color>";
                    partOne = results[i].Substring(0, index);
                    partThree = results[i].Substring(index + inputField.text.Length);

                    string total = partOne + partTwo + partThree;

                    possibleCommands[i] = total;
                }
            }

            possibleCommandsGUI.text = "";

            foreach (string command in possibleCommands)
            {
                possibleCommandsGUI.text += command + "\n";
            }
        }

        private void AppendToLastInputs()
        {
            if (lastInputs[0] == inputField.text)
            {
                return;
            }

            string[] newArray = new string[50];

            newArray[0] = inputField.text;

            for (int i = 0; i < 49; i++)
            {
                newArray[i + 1] = lastInputs[i];
            }

            lastInputs = newArray;
        }

        private void UpdateLog()
        {
            log.text = "";

            for (int i = 49; i >= 0; i--)
            {
                if (Console.logs[i] != "" && Console.logs[i] != string.Empty && Console.logs[i] != null)
                    log.text += Console.logs[i] + "\n";
            }
        }

        private void HandleUnityLog(string logString, string stackTrace, LogType type)
        {
            string hex = "";
            bool bold = false;
            float fontSize = 17;
            string prefix = "Unity Log";

            if (type == LogType.Exception)
            {
                bold = true;
                fontSize = 22;
            }

            if (logString.Contains("[BASTION]"))
            {
                prefix = "";
            }

            switch (type)
            {
                case LogType.Error:
                    hex = "ff534a";
                    break;
                case LogType.Warning:
                    hex = "ffc107";
                    break;
                case LogType.Log:
                    hex = "f0f0f0";
                    break;
                case LogType.Exception:
                    hex = "ff534a";
                    prefix = "EXCEPTION";
                    break;
                case LogType.Assert:
                    hex = "ff534a";
                    break;
            }

            Console.Log(logString, prefix, hex, bold, fontSize);
        }
    }
}