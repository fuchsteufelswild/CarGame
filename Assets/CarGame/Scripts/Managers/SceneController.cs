/* When a wave finishes, checks the current scene index bounds.
 * Depending on the result unloads current scene and loads
 * the next one.
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : ManagerBase,
                               IGameManager
{
    [System.Serializable]
    public struct SceneData
    {
        public string sceneName;
        public int startWave;
        public int endWave;
    }

    public SceneData CurrentSceneData => m_CurrentScene;

    [SerializeField] SceneData[] m_SceneDatas;

    bool m_IsSceneLoaded = false;
    SceneData m_CurrentScene;

    private void Awake()
    {
        EventManager.AddListener<int>(MissionEvents.WAVE_FINISHED, OnWaveFinished);
    }

    IEnumerator LoadSceneRoutine(AsyncOperation[] loadOperations, bool responsibleForLoading)
    {
        EventManager.NotifyEvent(SceneEvents.SCENE_LOAD_STARTED);
        if (responsibleForLoading)
            EventManager.NotifyEvent(LoadingEvents.LOADING_STARTED);

        int targetCount = loadOperations.Length;
        int maxProgress = targetCount * 100;
        while (true)
        {
            int currentDoneCount = 0;
            int currentProgress = 0;

            for (int i = 0; i < targetCount; ++i)
            {
                if (loadOperations[i].isDone)
                    ++currentDoneCount;

                currentProgress += (int)(loadOperations[i].progress * 100);
            }

            if (responsibleForLoading)
            {
                EventManager.NotifyEvent<int, int>(LoadingEvents.LOADING_PROGRESS, currentProgress, maxProgress);
            }

            if (currentDoneCount == targetCount)
                break;

            yield return null;
        }

        if (responsibleForLoading)
        {
            EventManager.NotifyEvent(LoadingEvents.LOADING_FINISHED);
        }
        EventManager.NotifyEvent(SceneEvents.SCENE_LOAD_FINISHED);
    }

    void LoadNewScene(SceneData newScene)
    {
        List<AsyncOperation> operations = new List<AsyncOperation>();

        if (m_IsSceneLoaded)
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(m_CurrentScene.sceneName);
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(newScene.sceneName, LoadSceneMode.Additive);

            operations.Add(unloadOperation);
            operations.Add(loadOperation);
        }
        else
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(newScene.sceneName, LoadSceneMode.Additive);
            operations.Add(loadOperation);
        }

        StartCoroutine(LoadSceneRoutine(operations.ToArray(), m_IsSceneLoaded));
        m_CurrentScene = newScene;
        m_IsSceneLoaded = true;
    }

    public void FindAndLoadScene(int wave)
    {
        SceneData sceneDataToLoad = default;
        for (int i = 0; i < m_SceneDatas.Length; ++i)
        {
            if (wave >= m_SceneDatas[i].startWave &&
                wave < m_SceneDatas[i].endWave)
            {
                sceneDataToLoad = m_SceneDatas[i];
                break;
            }
        }

        LoadNewScene(sceneDataToLoad);
    }

    void OnWaveFinished(int wave)
    {
        if (wave + 1 > m_CurrentScene.endWave)
        {
            FindAndLoadScene(wave + 1);
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

        Debug.Log("Scene Manager Started");

        yield return null;

        m_Status = ManagerStatus.STARTED;
    }

    void IGameManager.UpdateData(object data)
    {
        // FindAndLoadScene(1);
        if (data == null) return;
    }
}
