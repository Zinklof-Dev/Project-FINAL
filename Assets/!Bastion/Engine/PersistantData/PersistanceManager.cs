using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Bastion
{
    public static class PersistanceManager
    {
        public static bool Save(T data, string fileName = "save", string fileExtension = ".dat", bool useEncryption = false, bool verbose = false) where T : class
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

        public static T load<T>(string fileName = "save", string fileExtension = ".dat", bool useEncryption = false, bool verbose = false) where T : class
        {
            return null;   
        }

        private static string Encrypt(string input, string codeword)
        {
            byte[] bytes = new byte[input.length * 2];

            foreach (char c in input)
            {
                foreach(byte b in BitConverter.GetBytes(c))
                {
                    b = RotateRight(b, 6);
                    b = RotateLeft(b, 2);
                    b = RotateLeft(b, 6);

                    b = RotateRight(b, 7);
                    b = RotateRight(b, 3);
                    b = RotateLeft(b, 4);
                }
            }

            return null;
        }

        private static byte RotateRight(byte byte, int bits)
        {
            bits %= 8;

            return (byte)((byte >> bits) | (byte << (8 - bits)));
        }

        private static byte RotateLeft(byte byte, int bits)
        {
            bits %= 8

            return (byte)((byte << bits) | (byte >> (8 - bits)));
        }
    }
}