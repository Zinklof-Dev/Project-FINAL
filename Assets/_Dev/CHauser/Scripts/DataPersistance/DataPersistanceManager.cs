using UnityEngine;
using ZinklofDev.ConsoleV2;
using System.Collections.Generic;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    GameData gameData = new GameData();
    static DataPersistanceManager instance;
    List<IDataPersistance> dataPersistanceObjects;
    private FileDataHandler dataHandler = new FileDataHandler();

    [Header("Data Persistance (Game Data) Settings")]
    [SerializeField] string fileName = "gameData.json";
    [SerializeField] bool useEncryption = false;
    [SerializeField] bool autoLoadOnStart = false;
    [SerializeField] bool autoSaveOnQuit = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(Application.dataPath);

        // Ensures that there is only one instance of the DataPersistanceManager
        instance = FindFirstObjectByType<DataPersistanceManager>();
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        dataPersistanceObjects = FindAllDataPersistanceObjects();

        if(autoLoadOnStart)
        {
            Load();
        }
    }

    private void OnApplicationQuit()
    {
        if(autoSaveOnQuit)
        {
            Save();
        }
    }

    [Command("loads the game data")]
    public static void Load()
    {
        // Loads saved data into the gameData variable
        instance.gameData = instance.dataHandler.Load(Application.persistentDataPath, instance.fileName, instance.useEncryption);

        // If no saved data exists, initializes a new gameData object
        if (instance.gameData == null)
        {
            NewGame();
            return;
        }
        // Push game data to all other scripts that need it
        foreach (IDataPersistance dataPersistanceObj in instance.dataPersistanceObjects)
        {
            dataPersistanceObj.LoadData(instance.gameData);
        }
    }
    [Command("saves the game data")]
    public static void Save()
    {
        foreach (IDataPersistance dataPersistanceObj in instance.dataPersistanceObjects)
        {
            dataPersistanceObj.SaveData(ref instance.gameData);
        }

        instance.dataHandler.Save(instance.gameData, Application.persistentDataPath, instance.fileName, instance.useEncryption);
    }
    [Command("loads new game data")]
    public static void NewGame()
    {
        instance.gameData = new GameData();

        // Push game data to all other scripts that need it
        foreach (IDataPersistance dataPersistanceObj in instance.dataPersistanceObjects)
        {
            dataPersistanceObj.LoadData(instance.gameData);
        }
    }

    [Command("deletes the saved game data file")]
    public static void DeleteSaveData()
    {
        instance.dataHandler.DeleteSaveData(Application.persistentDataPath, instance.fileName, instance.useEncryption);
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistanceObjects);
    }


    // For Writing and Reading from editable JSON file

    [Command("loads the names list")]
    public static void LoadNamesList()
    {

    }
}

public interface IDataPersistance
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
