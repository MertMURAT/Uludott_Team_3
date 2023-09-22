using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{

    [SerializeField] private string FileName;
    [SerializeField] private bool useEncryption;
    [SerializeField] bool initializeDataIfNull = false;
    [SerializeField] bool saveGameObjectIfDisabled = true;

    public string _selectedProfileID;
    public bool isCheckPointReached = false;


    List<IDataPersistence> dataPersistenceObjects;
    Coroutine _checkPointSaveCoroutine;




    public static DataPersistenceManager _instance { get; private set; }
    FileHandler fdataHandler;
    GameData gameData;

    private void Start()
    {
        if (!DataPersistenceManager._instance.HasGameData()) ; // deactivate the continue button.
    }

    private void OnEnable()
    {
        // sceneLoaded will start after the `OnEnable()` method and before the `Start()` method

        SceneManager.sceneLoaded += OnSceneLoaded;

        if(_checkPointSaveCoroutine != null)
                StopCoroutine(CheckPointSave());

        DataPersistenceManager._instance.LoadGame();

        //_checkPointSaveCoroutine = StartCoroutine(CheckPointSave());

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene , LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }



    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Multiple instance of the singleton data structure");
            Destroy(this.gameObject);
            return;
        }
        else { _instance = this; DontDestroyOnLoad(_instance); }
        this.fdataHandler = new FileHandler(Application.persistentDataPath, FileName, useEncryption);
    }

    /*
    private void Start()
    {
        // moved to Awake()
        this.fdataHandler = new FileHandler(Application.persistentDataPath, FileName , useEncryption); 

        
        // Moved to OnSceneLoaded
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    */

    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {

       
        this.gameData = fdataHandler.Load(DataPersistenceManager._instance._selectedProfileID);

        // 1. Read the compressed file by using filehandler 
        // create a new GameData if no data file found 

        if (this.gameData == null && initializeDataIfNull)
        {
            Debug.LogWarning("No GameData was found. New Game Data must be initialized through 'NewGame' option");
            NewGame();
        }

        if (this.gameData == null)
        {
            Debug.LogWarning("No GameData was found. New Game Data must be initialized through 'NewGame' option");
            return;
        }

        // 2. initialise GameData() based on the data that comes from the filehandler.
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

    }

    public void SaveGame()
    {

        if (this.gameData == null)
        {
            Debug.LogWarning("No GameData was found. New Game Data must be initialized through 'NewGame' option//SaveGame()");
            return;
        }

        // 1. Read the GameData Script
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        // 2. Convert GameData Script into a compressed file format
        fdataHandler.Save(DataPersistenceManager._instance._selectedProfileID , gameData);
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

    public Dictionary<string , GameData> GetAllProfilesGameData()
    {
        return this.fdataHandler.LoadAllProfile();
    }


    public IEnumerator CheckPointSave()
    {
        while (true)
        {
            yield return new WaitUntil(()=> { return isCheckPointReached; });
            SaveGame();
            isCheckPointReached = false;
        }
    }

    public bool HasGameData() { return this.gameData != null; }

}
