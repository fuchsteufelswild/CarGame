using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Enums.Input;

public class UIPressedInputButton : UIBaseInputButton,
                                    IPointerDownHandler,
                                    IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        EventManager.NotifyEvent<UIBaseInputButton, ButtonTriggerType>(m_InputEventCode, this, ButtonTriggerType.DOWN);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EventManager.NotifyEvent<UIBaseInputButton, ButtonTriggerType>(m_InputEventCode, this, ButtonTriggerType.UP);
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(UIPressedInputButton))]
public class UIPressedInputButtonEditor : Editor
{
    UIBaseInputButtonEditor m_BaseEditor;

    private void OnEnable()
    {   
        m_BaseEditor = new UIBaseInputButtonEditor();
        m_BaseEditor.Init(target as UIPressedInputButton);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        m_BaseEditor.InspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }
}

#endif

