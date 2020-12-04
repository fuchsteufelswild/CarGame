/* Manager that is responsible for saving and 
 * restoring game data. Can be used if save
 * functionality is needed.
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : ManagerBase,
                           IGameManager
{
    Mutex m_SaveMutex;

    string m_FilePath = "";

    ManagerStatus IGameManager.GetStatus() => base.GetStatus();

    bool IGameManager.IsReady() => base.IsReady();

    private void Awake()
    {
        EventManager.AddListener(SaveEvents.SAVE_GAME_STATE, SaveGameData);
        EventManager.AddListener(SaveEvents.GAME_RESET, DeleteSave);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(SaveEvents.SAVE_GAME_STATE, SaveGameData);
        EventManager.RemoveListener(SaveEvents.GAME_RESET, DeleteSave);
    }

    void DeleteSave() =>
        File.Delete(m_FilePath);


    object IGameManager.GetData()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IGameManager.Init()
    {
        m_SaveMutex = new Mutex();

        m_Status = ManagerStatus.LOADING;

        m_FilePath = Path.Combine(Application.persistentDataPath, "GameData.dat");
        // m_FilePath = Path.Combine(Application.dataPath, "GameData.dat");

        Debug.Log("File Path " + m_FilePath);

        LoadGameData();

        yield return null;

        Debug.Log("Data Manager Started");

        m_Status = ManagerStatus.STARTED;
    }

    void IGameManager.UpdateData(object data)
    {
        throw new System.NotImplementedException();
    }

    public void SaveGameData()
    {
        // Only one save at a time
        m_SaveMutex.WaitOne();

        var currentState = new Dictionary<string, object>();

        currentState.Add("MissionState", ((IGameManager)Managers.MissionManager).GetData());

        FileStream file = File.Open(m_FilePath, FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, currentState);

        file.Close();

        m_SaveMutex.ReleaseMutex();
    }

    public void LoadGameData()
    {
        // Load Data From File and 
        // Update Managers

        if (!File.Exists(m_FilePath))
        {
            ((IGameManager)Managers.MissionManager).UpdateData(null);
            return;
        }

        Dictionary<string, object> gameState;
        FileStream file = File.Open(m_FilePath, FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        gameState = formatter.Deserialize(file) as Dictionary<string, object>;
        file.Close();
        
        ((IGameManager)Managers.MissionManager).UpdateData(gameState["MissionState"]);

        // EventMessenger.NotifyEvent(SaveEvents.LOADING_SAVE_COMPLETED);
    }
}
