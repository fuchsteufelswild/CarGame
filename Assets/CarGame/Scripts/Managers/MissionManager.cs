using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : ManagerBase,
                              IGameManager
{
    [SerializeField] float m_WaveTimer = 2.0f;

    private int m_CurrentWaveIndex = 1;
    private Level m_CurrentLevel = null;

    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    void PrepareForNewWave()
    {
        Time.timeScale = 0;
        // Signal UI for timer activation
        // Start Coroutine
    }

    IEnumerator TimerRoutine()
    {
        float timer = 0.0f;

        while(timer < m_WaveTimer)
        {
            yield return waitForEndOfFrame;
            timer += Time.deltaTime;
            // Update UI timer
        }

        if(++m_CurrentWaveIndex <= Managers.SceneController.CurrentSceneData.endWave)
        {
            SetNewWave();
        }

        EventManager.NotifyEvent<int>(MissionEvents.WAVE_FINISHED, m_CurrentWaveIndex - 1);
    }

    void LevelChanged()
    {
        m_CurrentLevel = FindObjectOfType<Level>();
        SetNewWave();
    }

    void SetNewWave()
    {
        // Get Entrance Point
        // Set player here
    }

    ManagerStatus IGameManager.GetStatus() => base.GetStatus();

    bool IGameManager.IsReady() => base.IsReady();

    object IGameManager.GetData()
    {
        return null;
    }

    IEnumerator IGameManager.Init()
    {
        m_Status = ManagerStatus.LOADING;

        EventManager.AddListener(SceneEvents.SCENE_LOAD_FINISHED, LevelChanged);
        EventManager.AddListener(MissionEvents.WAWE_COMPLETE_SIGNAL, PrepareForNewWave);

        Debug.Log("Mission Manager Started");

        yield return null;

        m_Status = ManagerStatus.STARTED;
    }

    void IGameManager.UpdateData(object data)
    {
        if(data == null)
        {
            Managers.SceneController.FindAndLoadScene(m_CurrentWaveIndex);
            return;
        }
        return;
    }
}
