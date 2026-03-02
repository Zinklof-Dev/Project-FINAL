using UnityEngine;
using TMPro;
    
namespace Bastion.LocalizationEditor
{
    public class FileViewer : MonoBehaviour
    {
        [SerializeField] TMP_Text text;

        public void Start()
        {
            PushNewData(text.text.Split("\n"));
        }

        public void PushNewData(string[] lines)
        {
            text.text = "";


            foreach (string s in lines)
            {
                text.text += (Localization.ColorEditorLine(s) + "\n");
            }
        }
    }
}