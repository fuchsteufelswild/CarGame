using UnityEngine;
using UnityEngine.UI;

public class UISceneLoader : MonoBehaviour
{
    [SerializeField] Image m_Background;
    [SerializeField] UIDynamicFillBar m_LoadingBar;

    private void Awake()
    {
        EventManager.AddListener(LoadingEvents.LOADING_STARTED, StartLoading);
        EventManager.AddListener(LoadingEvents.LOADING_FINISHED, LoadingFinished);
        EventManager.AddListener<int, int>(LoadingEvents.LOADING_PROGRESS, UpdateLoadingBar);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(LoadingEvents.LOADING_STARTED, StartLoading);
        EventManager.RemoveListener(LoadingEvents.LOADING_FINISHED, LoadingFinished);
        EventManager.RemoveListener<int, int>(LoadingEvents.LOADING_PROGRESS, UpdateLoadingBar);
    }

    void StartLoading()
    {
        m_LoadingBar.SetFillAmount(0);
        m_Background.gameObject.SetActive(true);
        m_LoadingBar.gameObject.SetActive(true);
    }

    void LoadingFinished()
    {
        m_Background.gameObject.SetActive(false);
        m_LoadingBar.gameObject.SetActive(false);
    }

    void UpdateLoadingBar(int numberReady, int totalNumber) =>
        m_LoadingBar.SetFillAmount(numberReady / (float)totalNumber);
}
