using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseScreen : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image m_Image;

    private void Start()
    {
        // m_Image = GetComponentInChildren<UnityEngine.UI.Image>();

        EventManager.AddListener(MissionEvents.START_NEW_WAVE, delegate 
        {
            EventManager.NotifyEvent(MissionEvents.GAME_PAUSED);
            m_Image.gameObject.SetActive(true);
        });
    }

    public void UnpauseCommand()
    {
        m_Image.gameObject.SetActive(false);
        EventManager.NotifyEvent(MissionEvents.GAME_UNPAUSED);
    }
}
