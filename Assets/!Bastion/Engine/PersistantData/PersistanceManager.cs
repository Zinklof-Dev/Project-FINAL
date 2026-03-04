using System;
using System.IO;
using UnityEngine;

namespace Bastion
{
    public static class PersistanceManager
    {
        public bool Save(T data, string fileName = "save", string fileExtension = ".dat", bool useEncryption = false, bool verbose = false) where T : class
        {
            string logPrefix = $"{Branding.engineLogPrefix}[PersistanceManager.Save ]"

            if (fileExtension[0] != '.')
            {
                if (verbose)
                    Debug.Log(logPrefix + "file extension did not start with \".\"! a \".\" was added for you!");

                fileExtension = "." + fileExtension;
            }

            if (fileName == "blank" || fileName == "")
            {
                //fileName = data.name;
            }

            string path = Application.persistentDataPath + "/saves/" + fileName + fileExtension;

            string serializedData = JsonUtility.ToJson(data);

            if (useEncryption)
            {
                serializedData = EncryptDecrypt(serializedData, "ExampleForNow");
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(serializedData);
                }

                return true
            }
            catch
            {
                //uh oh
                Debug.LogWarning(logPrefix + "ran into an exception that it has no way of handling!\n" + e.Message + "\n" + e.StackTrace);
                return false;
            }
        }

        public T load<T>(string fileName = "save", string fileExtension = ".dat", bool useEncryption = false, bool verbose = false) where T : class
        {
            return null;   
        }

        private string Encrypt(string input, string codeword)
        {
            return input; // implement at home
        }
    }
}