using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Bastion.LocalizationEditor
{
    public class FileExplorer : MonoBehaviour
    {
        [SerializeField] GameObject feContainer;
        [SerializeField] TMP_Text dirtext;
        [SerializeField] GameObject folderPrefab;
        [SerializeField] GameObject filePrefab;

        private EditorMain em;

        public string forcedDir;
        public string addedDir;
        public string currentDir;

        public void ContactFE(EditorMain em)
        {
            this.em = em;

            forcedDir = Application.dataPath + "/JSON/Localization";
            addedDir = "";
            dirtext.text = "Localization/" + addedDir;

            currentDir = forcedDir + addedDir;

            CreateObjs(currentDir);
        }

        public void ExitDir()
        {
            Debug.Log("entered exit dir");

            string[] dirs = addedDir.Split("/");

            string returnDir = "";

            for(int i = 0; i < dirs.Length-1; i++)
            {
                returnDir += "/" + dirs[i];
            }

            addedDir = returnDir;

            currentDir = forcedDir + addedDir;

            dirtext.text = "Localization/" + addedDir;
            CreateObjs(currentDir);
        }

        public void EnterDir(string dirName)
        {
            addedDir += dirName;

            currentDir = forcedDir + addedDir;

            dirtext.text = "Localization/" + addedDir;
            CreateObjs(currentDir);
        }

        public void GetFileFromCurrentDir(string name)
        {
            List<string> linesList = new List<string>();

            using (StreamReader sr = new StreamReader(currentDir + "/" + name))
            {
                string line = null;

                while((line = sr.ReadLine()) != null)
                {
                    linesList.Add(line);
                }
            }

            em.ed.PushNewData(linesList.ToArray());
        }

        private void DestroyObjs()
        {
            foreach (Transform child in feContainer.transform)
            {
                if (child.GetComponent<FileExplorerButton>() != null)
                Destroy(child.gameObject);
            }
        }

        private void CreateObjs(string path)
        {
            Debug.Log(path);

            DestroyObjs();

            string[] refs = GetViewForFolder(path);

            foreach (string r in refs)
            {
                if (r.ToLower().Contains(".loc"))
                    CreateFileObj(r);
                else
                    CreateFolderObj(r);
            }
        }

        private void CreateFolderObj(string folder)
        {
            GameObject go = Instantiate(folderPrefab);
            go.GetComponent<FileExplorerButton>().setbasics(folder, this);
            go.transform.SetParent(feContainer.transform, false);
        }

        private void CreateFileObj(string file)
        {
            GameObject go = Instantiate(filePrefab);
            go.GetComponent<FileExplorerButton>().setbasics(file, this);
            go.transform.SetParent(feContainer.transform, false);
        }

        private string[] GetViewForFolder(string path)
        {
            Debug.Log(path);
            string[] files = Directory.GetFiles(path, "*.loc", SearchOption.TopDirectoryOnly);
            Debug.Log(path);
            string[] folders = Directory.GetDirectories(path + "/");

            List<string> itemList = new List<string>();

            foreach (string folder in folders)
            {
                itemList.Add("/" + folder.Split("/")[folder.Split("/").Length-1]);
            }
            foreach (string file in files)
            {
                itemList.Add(file.Split("/")[file.Split("/").Length - 1]);
            }

            return itemList.ToArray();
        }
    }
}