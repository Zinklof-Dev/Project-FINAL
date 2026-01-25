using UnityEngine;
using ZinklofDev.ConsoleV2;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class DataPersistenceManager : MonoBehaviour
{
    GameData gameData = new GameData();
    public NamesList namesList = null;

    public static DataPersistenceManager instance;
    List<IDatapersistence> datapersistenceObjects;
    private FileDataHandler dataHandler = new FileDataHandler();

    [Header("Data persistence (Game Data) Settings")]
    [SerializeField] string fileName = "gameData.json";
    [SerializeField] bool useEncryption = false;
    [SerializeField] bool autoLoadOnStart = false;
    [SerializeField] bool autoSaveOnQuit = false;

    [Header("Data persistence (Editable JSON) Settings")]
    [SerializeField] string editableJSONDataFolderName = "JSON";
    [SerializeField] string namesListFileName = "namesList.json";
    [SerializeField] bool namesListUseEncryption = false;
    [SerializeField] bool autoLoadNamesListOnStart = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         //Debug.Log(Application.dataPath);

        // Ensures that there is only one instance of the DatapersistenceManager
        instance = FindFirstObjectByType<DataPersistenceManager>();
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        datapersistenceObjects = FindAllDatapersistenceObjects();

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
        instance.gameData = instance.dataHandler.Load<GameData>(Application.persistentDataPath, instance.fileName, instance.useEncryption);

        // If no saved data exists, initializes a new gameData object
        if (instance.gameData == null)
        {
            NewGame();
            return;
        }
        // Push game data to all other scripts that need it
        foreach (IDatapersistence datapersistenceObj in instance.datapersistenceObjects)
        {
            datapersistenceObj.LoadData(instance.gameData);
        }
        Console.Log("Game data loaded.", "DataPersistenceManager");
    }
    [Command("saves the game data")]
    public static void Save()
    {
        foreach (IDatapersistence datapersistenceObj in instance.datapersistenceObjects)
        {
            datapersistenceObj.SaveData(ref instance.gameData);
        }

        instance.dataHandler.Save<GameData>(instance.gameData, Application.persistentDataPath, instance.fileName, instance.useEncryption);
        Console.Log("Game data saved.", "DataPersistenceManager");
    }
    [Command("loads new game data")]
    public static void NewGame()
    {
        instance.gameData = new GameData();

        // Push game data to all other scripts that need it
        foreach (IDatapersistence datapersistenceObj in instance.datapersistenceObjects)
        {
            datapersistenceObj.LoadData(instance.gameData);
        }
        Console.Log("New game data initialized.", "DataPersistenceManager");
    }

    [Command("deletes the saved game data file")]
    public static void DeleteSaveData()
    {
        instance.dataHandler.DeleteSaveData(Application.persistentDataPath, instance.fileName, instance.useEncryption);

        Console.Log("Saved game data file deleted.", "DataPersistenceManager");
    }

    private List<IDatapersistence> FindAllDatapersistenceObjects()
    {
        IEnumerable<IDatapersistence> datapersistenceObjects = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDatapersistence>();
        return new List<IDatapersistence>(datapersistenceObjects);
    }


    // For Writing and Reading from editable JSON file

    // NAMES LIST

    [Command("loads the names list")]
    public static void LoadNamesList()
    {
        instance.namesList = instance.dataHandler.Load<NamesList>(Path.Combine(Application.dataPath, instance.editableJSONDataFolderName), instance.namesListFileName, instance.namesListUseEncryption);

        if(instance.namesList == null)
        {
            instance.namesList = new NamesList();
            Console.Log("Names list file not found, initialized new names list.", "DataPersistenceManager");
            return;
        }

        Console.Log("Names list loaded.");
    }

    [Command("saves the names list")]
    public static void SaveNamesList()
    {
        if(instance.namesList == null)
        {
            Console.Log("Names list is null, cannot save.", "DataPersistenceManager");
            return;
        }

        instance.dataHandler.Save<NamesList>(instance.namesList, Path.Combine(Application.dataPath, instance.editableJSONDataFolderName), instance.namesListFileName, instance.namesListUseEncryption);
        Console.Log("Names list saved.", "DataPersistenceManager");
    }
    [Command("Add a name to the first names list")]
    public static void AddFirstNameToNamesList(string name, bool pushToSave)
    {
        instance.namesList.firstNames.Add(name);
        Console.Log("Added first name: " + name, "DataPersistenceManager");
        if (pushToSave)
        {
            SaveNamesList();
            Console.Log("Names list saved after adding first name.", "DataPersistenceManager");
        }
    }
    [Command("Add a name to the last names list")]
    public static void AddLastNameToNamesList(string name, bool pushToSave)
    {
        instance.namesList.lastNames.Add(name);
        Console.Log("Added last name: " + name, "DataPersistenceManager");
        if (pushToSave)
        {
            SaveNamesList();
            Console.Log("Names list saved after adding last name.", "DataPersistenceManager");
        }
    }
    [Command("Add a combined name to the last names list")]
    public static void AddCombinedNameToNamesList(string name, bool pushToSave)
    {
        instance.namesList.comboNames.Add(name);
        Console.Log("Added combo name: " + name, "DataPersistenceManager");
        if (pushToSave)
        {
            SaveNamesList();
            Console.Log("Names list saved after adding combo name.", "DataPersistenceManager");
        }
    }
    [Command("Prints the names lists to console")]
    public static void PrintNamesLists()
    {
        Console.Log("First Names:", "DataPersistenceManager");

        foreach(string name in instance.namesList.firstNames)
        {
            Console.Log(name, "DataPersistenceManager");
        }
        Console.Log("Last Names:", "DataPersistenceManager");

        foreach (string name in instance.namesList.lastNames)
        {
            Console.Log(name, "DataPersistenceManager");
        }

        Console.Log("Combined Names:", "DataPersistenceManager");
        foreach (string name in instance.namesList.comboNames)
        {
            Console.Log(name, "DataPersistenceManager");
        }
    }
}


public interface IDatapersistence
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
