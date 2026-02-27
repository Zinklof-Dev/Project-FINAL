using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using ZinklofDev;

namespace ZinklofDev.LocalizationEditor
{
    public class FileExplorer : MonoBehaviour
    {
        [SerializeField] GameObject feContainer;

        private EditorMain em;

        string currentDir;

        public void ContactFE(EditorMain em)
        {
            this.em = em;

            currentDir = Applicaton.dataPath + "/JSON/Localization";
        }

        public void ExitDir()
        {
            string[] dirs = currentDir.Split("/");

            string returnDir = "";

            returnDir += dirs[0];

            for(int i = 1; i < dirs.length-1; i++)
            {
                returnDir += "/" + dirs[i];
            }

            CreateObjs(currentDir);
        }

        public void EnterDir(string dirName)
        {
            currentDir += "/" + dirName;

            CreateObjs(currentDir);
        }

        private void GetFileFromCurrentDir(string name)
        {
            List<string> linesList = new List<string>();

            using (StreamReader sr = new StreamReader(currentDir + "/" + name))
            {
                while((line = sr.ReadLine()) != null)
                {
                    linesList.Add(line);
                }
            }

            em.fv.PushNewData(linesList.ToArray());
        }

        private void DestroyObjs()
        {
            foreach (Transform child in feContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void CreateObjs(string path)
        {
            string[] refs = GetViewForFolder(path);

            //code to insantiate the back button

            foreach (string r in refs)
            {
                if (r.Contains(".loc"))
                    CreateFileObj(r);
                else
                    createFolderObj(r)
            }
        }

        private void CreateFolderObj(string folder)
        {
            
        }

        prviate void CreateFileObj(string file)
        {
            
        }

        private string[] GetViewForFolder(string path)
        {
            string[] files = Directory.GetFiles(path, "*.loc", SearchOption.AllDirectories);
            string[] folders = Director.GetDirectories(path, SearchOption.AllDirectories);

            List<string> itemList = new List<string>();

            itemList.Add("\\...")
            foreach (string folder in folders)
            {
                itemList.Add("\\" + folder.Split("/")[folder.Split("/").length-1]);
            }
            foreach (string file in files)
            {
                itemList.add(file.Split("/")[file.Split("/").length-1])
            }

            return itemList.ToArray();
        }
    }
}