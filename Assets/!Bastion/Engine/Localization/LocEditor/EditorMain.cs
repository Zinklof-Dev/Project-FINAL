using UnityEngine;

namespace Bastion.LocalizationEditor
{
    public class EditorMain : MonoBehaviour
    {
        public FileViewer fv;
        public FileExplorer fe;
        public FileEditor ed;

        private void Start()
        {
            fe.ContactFE(this);
            ed.ContactED(this);
        }

        public void SendDataToFV(string text)
        {
            fv.PushNewData(text.Split('\n'));
        }
    }
}