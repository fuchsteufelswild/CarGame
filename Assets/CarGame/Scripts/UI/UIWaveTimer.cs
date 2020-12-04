using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWaveTimer : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text m_TimerText;

    private void Start()
    {
        // m_TimerText = GetComponentInChildren<UnityEngine.UI.Text>();

        EventManager.AddListener<float>(MissionEvents.WAVE_TIMER_PROGRESS, (value) => m_TimerText.text = value.ToString("F1"));
        EventManager.AddListener(MissionEvents.WAVE_COMPLETE_SIGNAL, delegate
        {
            m_TimerText.gameObject.SetActive(true);
            m_TimerText.text = Managers.MissionManager.MaxWaveTimer.ToString("F1");
        });
        EventManager.AddListener(MissionEvents.START_NEW_WAVE, () => m_TimerText.gameObject.SetActive(false));
    }
}
