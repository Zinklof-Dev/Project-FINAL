using UnityEngine;
using ZinklofDev.ConsoleV2;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class DataPersistenceManager : MonoBehaviour
{
    GameData gameData = new GameData();
    public NamesList namesList = null;
    public BackgroundsList backgroundsList = null;

    public static DataPersistenceManager instance;
    List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler = new FileDataHandler();

    [Header("Data Persistence (Game Data) Settings")]
    [SerializeField] string fileName = "BOTDsavedata.zink";
    [SerializeField] bool useEncryption = false;
    [SerializeField] bool autoLoadOnStart = false;
    [SerializeField] bool autoSaveOnQuit = false;

    [Header("Data Persistence (Editable JSON) Settings")]
    [SerializeField] string editableJSONDataFolderName = "JSON";
    [SerializeField] string namesListFileName = "namesList.json";
    [SerializeField] string backgroundsListFileName = "backgroundsList.json";
    [SerializeField] bool namesListUseEncryption = false;
    // [SerializeField] bool autoLoadNamesListOnStart = false;

    public static bool newGameOnStart = false;
    public static bool loadGameOnStart = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ensures that there is only one instance of the Data Persistence Manager
        instance = FindFirstObjectByType<DataPersistenceManager>();
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        dataPersistenceObjects = FindAllDataPersistenceObjects();

        foreach (IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            Debug.Log(dataPersistence.ToString());
        }

        if (loadGameOnStart)
        {
            loadGameOnStart = false;
            Load();
        }

        if (newGameOnStart)
        {
            newGameOnStart = false;
            NewGame();
        }

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
        foreach (IDataPersistence dataPersistenceObj in instance.dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(instance.gameData);
        }
        Console.Log("Game data loaded.", "DataPersistenceManager");
    }
    [Command("saves the game data")]
    public static void Save()
    {
        foreach (IDataPersistence dataPersistenceObj in instance.dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref instance.gameData);
        }

        instance.dataHandler.Save<GameData>(instance.gameData, Application.persistentDataPath, instance.fileName, instance.useEncryption);
        Console.Log("Game data saved.", "DataPersistenceManager");
    }
    [Command("loads new game data")]
    public static void NewGame()
    {
        instance.gameData = new GameData();

        // Push game data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in instance.dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(instance.gameData);
        }
        Console.Log("New game data initialized.", "DataPersistenceManager");
    }

    [Command("deletes the saved game data file")]
    public static void DeleteSaveData()
    {
        instance.dataHandler.DeleteSaveData(Application.persistentDataPath, instance.fileName, instance.useEncryption);

        Console.Log("Saved game data file deleted.", "DataPersistenceManager");
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        List<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IDataPersistence>().ToList();
        return dataPersistenceObjects;
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

    // Backgrounds List

    [Command("loads the backgrounds list")]
    public static void LoadBackgroundsList()
    {
        instance.backgroundsList = instance.dataHandler.Load<BackgroundsList>(Path.Combine(Application.dataPath, instance.editableJSONDataFolderName), instance.backgroundsListFileName, false);
        if (instance.backgroundsList == null)
        {
            instance.backgroundsList = new BackgroundsList();
            Console.Log("Backgrounds list file not found, initialized new backgrounds list.", "DataPersistenceManager");
            return;
        }

        Console.Log("Backgrounds list loaded.");
    }

    [Command("saves the backgrounds list")]
    public static void SaveBackgroundsList()
    {
        if (instance.backgroundsList == null)
        {
            Console.Log("Backgrounds list is null, cannot save.", "DataPersistenceManager");
            return;
        }

        instance.dataHandler.Save<BackgroundsList>(instance.backgroundsList, Path.Combine(Application.dataPath, instance.editableJSONDataFolderName), instance.backgroundsListFileName, false);
        Console.Log("Backgrounds list saved.", "DataPersistenceManager");
    }
}


public interface IDataPersistence
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
