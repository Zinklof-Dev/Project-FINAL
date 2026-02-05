using UnityEngine;
using System.IO;
using System;

public class FileDataHandler
{
    private readonly string encryptionCodeWord = "EreATrhONESKUZcsW-0XvALthtuIF7MkqGPQm4pCiBIA8G8K_yZOK0yXD7puA6JhtQAc0lU-yvOk2MgMv_bDy9-weBO0C_Zy9UGQ7lhkt90kPgtIzex2r3CkqfKvpzn88E-gArsf-ze-aEaAySV8GmWLLApy0yGI4SEU6yRKB8o8fVYQ9Gu7_ncbEEuOo7Gn5ii32QiSyYXNncpE2aKH4YVWenIw";

    public void DeleteSaveData(string dataDirPath, string dataFileName, bool useEncryption)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error deleting save data file: " + fullPath + "\n" + e);
        }
    }


    public T Load<T>(string dataDirPath, string dataFileName, bool useEncryption) where T : class
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        T loadedData = null;
        if (File.Exists(fullPath))
        {
            string dataToLoad = "";

            try
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                if(useEncryption)
                {
                    dataToLoad = EncryptDecrypt(ref dataToLoad);
                }
                loadedData = JsonUtility.FromJson<T>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error reading data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save<T>(T data, string dataDirPath, string dataFileName, bool useEncryption) where T : class
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToSave = JsonUtility.ToJson(data, true);
            if (useEncryption)
            {
                dataToSave = EncryptDecrypt(ref dataToSave);
            }
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToSave);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving data to file: " + fullPath + "\n" + e);
        }
    }

    private string EncryptDecrypt(ref string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}

