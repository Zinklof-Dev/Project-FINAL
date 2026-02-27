using system;
using UnityEngine;
using TMPro;
using ZinklofDev;

namespace ZinklofDev.LocalizationEditor
{
    public class FileViewer : MonoBehaviour
    {
        [SerializeField] TMP_Text text;

        public void PushNewData(string[] lines)
        {
            foreach (string s in lines)
            {
                text.text += (Localization.ColorEditorLine(s) + "\n");
            }
        }
    }
}