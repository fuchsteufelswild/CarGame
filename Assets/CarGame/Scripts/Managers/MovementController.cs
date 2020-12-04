/* The class that will receive all the button inputs
 * from UIBaseInputButtonBase instances via events. 
 * Processes the type and triggers events
 */ 

using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Enums.Input;

public class MovementController : MonoBehaviour
{
    Dictionary<string, bool> m_InputStatus = new Dictionary<string, bool>();

    void TriggerInput(UIBaseInputButton button, ButtonTriggerType triggerType)
    {
        switch(triggerType)
        {
            case ButtonTriggerType.DOWN:
                m_InputStatus[button.ActionCode] = true;
                break;
            case ButtonTriggerType.UP:
                m_InputStatus[button.ActionCode] = false;
                break;
            case ButtonTriggerType.CLICK:
                EventManager.NotifyEvent(button.InputEventCode);
                break;
            default:
                break;
        }
    }

    void ResetStatus()
    {
        foreach (KeyValuePair<string, bool> pair in m_InputStatus)
            m_InputStatus[pair.Key] = false;
    }

    void Start()
    {
        string[] allButtonEventCodes = typeof(InputButtonEvents).GetConstantValues<string>().ToArray();
        for (int i = 0; i < allButtonEventCodes.Length; ++i)
            EventManager.AddListener<UIBaseInputButton, ButtonTriggerType>(allButtonEventCodes[i], TriggerInput);
        
        string[] allInputEventCodes = typeof(InputEvents).GetConstantValues<string>().ToArray();
        for (int i = 0; i < allInputEventCodes.Length; ++i)
            m_InputStatus.Add(allInputEventCodes[i], false);
        
        // EventManager.AddListener<int>(MissionEvents.WAVE_FINISHED, (num) => ResetStatus());
    }

    void Update()
    {
        if (!Managers.MissionManager.IsPaused)
        {
            foreach (KeyValuePair<string, bool> pair in m_InputStatus)
            {
                if (pair.Value)
                    EventManager.NotifyEvent(pair.Key);
            }
        }
    }
}
