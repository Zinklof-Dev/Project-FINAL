using UnityEngine;
using TMPro;
 
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
        string name;
        // variables
        string path
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
            nameBar.text = name;

            if (unsavedChanges)
                nameBar.text += "*";
        }

        public void PushChanges()
        {
            if (unsavedChanges = false)
            {
                unsavedChanges = true;
                UpdateUI();
            }

            em.SendDataToFV(inputField.text);
        }

        public void SaveCurrentData()
        {
            unsavedChanges = false();

            srting[] lines = inputField.text.Split("\n");

            using (StreamWriter sw = StreamWriter(path))
            {
                foreach (string s in lines)
                {
                    sw.WriteLine(line);
                }
            }

            UpdateUI();
        }
    }
}