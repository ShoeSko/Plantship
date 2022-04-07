using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public Data data; //Makes all values within the data accsesable
    string path => Application.dataPath + "/SaveData/saveData.json";
    string mobilePath => Application.persistentDataPath + "/SaveData/saveData.json";

    #region At the start
    private void Awake()
    {
        DontDestroyOnLoad(this); //Makes it so that all scenes can see it. (Potentialy other scrip
#if UNITY_STANDALONE || UNITY_EDITOR
        if (Directory.Exists(Application.dataPath + "/SaveData/"))
        {
            Load(); //Loads the game at the start.
            print("Data Loaded");
        }
        else
        {
            Directory.CreateDirectory(Application.dataPath + "/SaveData/");
        }
#elif UNITY_ANDROID
        if (Directory.Exists(Application.persistentDataPath + "/SaveData/"))
        {
            Load(); //Loads the game at the start.
            print("Data Loaded");
        }
        else
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveData/");
        }

#endif
    }
#endregion
    #region Save & Load
    [ContextMenu("Save")] //Makes a menu option in component section,right click to accsess.
    void Save()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        string json = JsonUtility.ToJson(data); //Writes the Data in Json Format
        File.WriteAllText(path, json);  //Stores the Json Data
#elif UNITY_ANDROID
        string json = JsonUtility.ToJson(data); //Writes the Data in Json Format
        File.WriteAllText(mobilePath, json);  //Stores the Json Data
#endif
    }
    [ContextMenu("Load")]//Makes a menu option in component section,right click to accsess.
    void Load()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        string json = File.ReadAllText(path); //Reads the Json Data
        Data loadedData = JsonUtility.FromJson<Data>(json); //Write the Json format in as readable data
#elif UNITY_ANDROID
        string json = File.ReadAllText(mobilePath); //Reads the Json Data
        Data loadedData = JsonUtility.FromJson<Data>(json); //Write the Json format in as readable data
#endif
        data = loadedData;
    }
#endregion
    #region When to save
    //Currently this can be called everytime a value is changed (Will prevent lost data from crash, might be heavy?)

    public void SaveTheData()
    {
        Save(); //Saves the data on every loaded level.
        print("Data Saved!");
    }
#endregion
}

#region The Data to save
[System.Serializable]
public class Data //Serializable class of data. All inf to be saved should be here.
{
    public bool isFirstRun;
    public bool hasPlayedTutorial;

    public float masterVolLevel;
    public float musicVolLevel;
    public float sfxVolLevel;

    public bool[] unitList;
    public bool[] beastList;
    public bool[] humanoidList;
    public bool[] monstrosityList;
    public bool[] bossList;
    public bool[] bossMeetList;

    public int[] progressValueList;

    public string lastScene;

    //public int minigamesFinished;
    //public int enemiesKilled;
    //public int cardsSummoned;

    public Data()
    {

    }

    //public Data(int minigamesFinished, int enemiesKilled, int cardsSummoned)
    //{


    //    this.minigamesFinished = minigamesFinished;
    //    this.enemiesKilled = enemiesKilled;
    //    this.cardsSummoned = cardsSummoned;
    //}
}
#endregion