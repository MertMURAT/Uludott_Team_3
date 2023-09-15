using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{

    [SerializeField] private string FileName;
    [SerializeField] private bool useEncryption;

    private List<IDataPersistence> dataPersistenceObjects;


    
    public static DataPersistenceManager _instance { get; private set; }
    FileHandler fdataHandler;
    GameData gameData;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Multiple instance of the singleton data structure");
        }
        else _instance = this;
    }

    private void Start()
    {
        this.fdataHandler = new FileHandler(Application.persistentDataPath, FileName , useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {

        this.gameData = fdataHandler.Load();

        // 1. Read the compressed file by using filehandler 
            // create a new GameData if no data file found 
        if (this.gameData == null)
        {
            Debug.LogWarning("No GameData was found. Initialising data to defaults");
            NewGame();
        }

        // 2. initialise GameData() based on the data that comes from the filehandler.
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        // 1. Read the GameData Script
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        // 2. Convert GameData Script into a compressed file format
        fdataHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }


    private void OnApplicationQuit()
    {
        SaveGame();
    }

}
