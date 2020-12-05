/* Manager to keep track of current game settings.
 * Responsible for handling wave start/end functionality,
 * keeping track of current level related data, and
 * game state.
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : ManagerBase,
                              IGameManager
{
    [SerializeField] float m_WaveTimer = 2.0f;
    [SerializeField] private PlayerCar m_PlayerCar;

    private Vector3 m_DefaultPlayerCarPosition = Vector3.zero;
    private int m_CurrentWaveIndex = 1;
    private Level m_CurrentLevel = null;
    private EntranceExitPair m_CurrentEntranceExitPair;

    private bool m_Paused = false;

    private List<CarBase> m_AllCars = new List<CarBase>(); // First, player car will be added to the list

    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    public float MaxWaveTimer => m_WaveTimer;
    public bool IsPaused => m_Paused;

    /* Freezes the game, feeds successful player record
     * into new ghost car then starts timer.
     */ 
    void PrepareForNewWave()
    {
        Time.timeScale = 0;
        m_Paused = true;

        GhostCar ghost = Managers.ObjectManager.AvailableGhostCar;
        ghost.SetData(m_PlayerCar.RecordArray, m_CurrentEntranceExitPair);
        ghost.UpdateParams(m_CurrentLevel);
        m_AllCars.Add(ghost);

        StartCoroutine(TimerRoutine());
    }

    IEnumerator TimerRoutine()
    {
        float timer = m_WaveTimer;

        while(timer > 0.0f)
        {
            timer -= Time.unscaledDeltaTime;
            EventManager.NotifyEvent<float>(MissionEvents.WAVE_TIMER_PROGRESS, timer);
            yield return null;
        }

        if(++m_CurrentWaveIndex <= Managers.SceneController.CurrentSceneData.endWave)
        {
            SetNewWave();
        }

        EventManager.NotifyEvent<int>(MissionEvents.WAVE_FINISHED, m_CurrentWaveIndex - 1);
    }

    void LevelChanged()
    {
        m_PlayerCar.transform.position = m_DefaultPlayerCarPosition;
        for (int i = 1; i < m_AllCars.Count; ++i)
            m_AllCars[i].gameObject.SetActive(false);

        m_AllCars.RemoveRange(1, m_AllCars.Count - 1);

        m_CurrentLevel = FindObjectOfType<Level>();
        SetNewWave();
    }

    void ResetCarTransform()
    {
        // if (m_Paused) return;
        m_Paused = true;

        for (int i = 0; i < m_AllCars.Count; ++i)
        {
            m_AllCars[i].ResetToInitialAttributes();
            m_AllCars[i].gameObject.SetActive(true);
        }

        EventManager.NotifyEvent(MissionEvents.START_NEW_WAVE);
        Time.timeScale = 1;
    }

    void SetNewWave()
    {
        if(m_CurrentEntranceExitPair != null)
            m_CurrentEntranceExitPair.gameObject.SetActive(false);

        m_CurrentEntranceExitPair = m_CurrentLevel.NextRandomEntraceExitPair;
        m_PlayerCar.SetData(m_CurrentEntranceExitPair);
        m_CurrentEntranceExitPair.gameObject.SetActive(true);

        ResetCarTransform();
    }

    private void FixedUpdate()
    {
        if(!m_Paused)
        {
            for(int i = 0; i < m_AllCars.Count; ++i)
            {
                m_AllCars[i].Tick(Time.deltaTime);
            }
        }
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
        EventManager.AddListener(MissionEvents.WAVE_COMPLETE_SIGNAL, PrepareForNewWave);
        EventManager.AddListener(MissionEvents.PLAYER_BUMP, ResetCarTransform);
        EventManager.AddListener(MissionEvents.GAME_UNPAUSED, () => m_Paused = false);

        if(m_PlayerCar == null)
        {
            m_PlayerCar = FindObjectOfType<PlayerCar>();
            if (m_PlayerCar == null)
                Debug.LogError("Player Car cannot be found.");
        }
        m_DefaultPlayerCarPosition = m_PlayerCar.transform.position;
        m_AllCars.Add(m_PlayerCar);

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
