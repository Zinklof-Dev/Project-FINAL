using UnityEngine;
using System.IO;
using System;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    bool useEncryption = false;
    private readonly string encryptionCodeWord = "ZinklofDevSuperSecretEncrpytionCodeWordForXOREncryption";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public void DeleteSaveData()
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


    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
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
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error reading data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
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
