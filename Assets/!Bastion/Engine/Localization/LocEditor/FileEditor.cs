using UnityEngine;
using TMPro;
using System.IO;

namespace Bastion.LocalizationEditor
{
    public class FileEditor : MonoBehaviour
    {
        //References
        [SerializeField] TMP_InputField inputField;
        [SerializeField] TMP_Text nameBar;
       
        // fully hidden References
        EditorMain em;
        // display variables
        string fileName = "";
        // variables
        string path;
        bool unsavedChanges = false;

        public void ContactED(EditorMain em)
        {
            this.em = em;
        }

        public void PushNewData(string[] data, string path)
        {
            this.path = path;

            inputField.text = "";

            foreach (string s in data)
            {
                inputField.text += s + "\n";
            }

            UpdateUI();
        }

        public void UpdateUI()
        {
            string[] split = path.Replace("\\", "/").Split('/');

            fileName = split[split.Length - 1];

            nameBar.text = fileName;

            if (unsavedChanges)
                nameBar.text += "*";
        }

        public void PushChanges()
        {
            if (unsavedChanges == false)
            {
                unsavedChanges = true;
                UpdateUI();
            }

            em.SendDataToFV(inputField.text);
        }

        public void SaveCurrentData()
        {
            unsavedChanges = false;

            string[] lines = inputField.text.Split("\n");

            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (string s in lines)
                {
                    sw.WriteLine(s);
                }
            }

            UpdateUI();
        }
    }
}