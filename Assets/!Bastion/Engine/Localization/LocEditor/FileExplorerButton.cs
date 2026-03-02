using TMPro;
using UnityEngine;

namespace Bastion.LocalizationEditor
{
    public class FileExplorerButton : MonoBehaviour
    {
        public FileExplorer fe;
        [SerializeField] TMP_Text text;
        [SerializeField] bool file;

        public void setbasics(string name, FileExplorer fe)
        {
            this.name = name;
            this.fe = fe;

            text.text = name;
        }

        public void clicked()
        {
            if (!file)
                fe.EnterDir(name);
            else
                fe.GetFileFromCurrentDir(name);
        }
    }
}
