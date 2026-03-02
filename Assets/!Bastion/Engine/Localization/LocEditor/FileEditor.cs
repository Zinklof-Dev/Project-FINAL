using UnityEngine;
using TMPro;
 
namespace Bastion.LocalizationEditor
{
    public class FileEditor : MonoBehaviour
    {
        [SerializeField] TMP_InputField inputField;

        EditorMain em;

        public void ContactED(EditorMain em)
        {
            this.em = em;
        }

        public void PushNewData(string[] data)
        {
            inputField.text = "";

            foreach (string s in data)
            {
                inputField.text += s + "\n";
            }
        }

        public void PushChanges()
        {
            em.SendDataToFV(inputField.text);
        }
    }
}