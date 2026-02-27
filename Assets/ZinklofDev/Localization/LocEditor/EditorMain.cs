using System;
using UnityEngine;
using ZinklofDev;

namespace ZinklofDev.localizationEditor
{
    public class EditorMain : MonoBehaviour
    {
        public FileViewer fv;
        public FileExplorer fe;

        private void Start()
        {
            fe.ContactFE(this);
        }
    }
}