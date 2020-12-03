/* Base Input class input event code describes
 * which kind of event it triggers and action
 * code represents concrete event code used by
 * agents of the game
 */ 

using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class UIBaseInputButton : MonoBehaviour
{
    [SerializeField] [HideInInspector] protected string m_InputEventCode;
    [SerializeField] [HideInInspector] protected string m_ActionEventCode;

    public string InputEventCode => m_InputEventCode;
    public string ActionCode => m_ActionEventCode;

    [HideInInspector] public int eventChoiceIndex = 0;

    public void UpdateEventCode(string newEventCode)
    {
        m_InputEventCode = newEventCode;
        m_ActionEventCode = m_InputEventCode.Remove(m_InputEventCode.Length - 7);
    }
}


#if UNITY_EDITOR
public class UIBaseInputButtonEditor
{
    UIBaseInputButton m_Target = null;

    string[] m_PossibleChoices;

    public void Init(UIBaseInputButton target)
    {
        m_Target = target;
    }

    public void InspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Event Code");
        
        m_PossibleChoices = typeof(InputButtonEvents).GetConstantValues<string>().ToArray();

        EditorGUI.BeginChangeCheck();
        m_Target.eventChoiceIndex = EditorGUILayout.Popup(m_Target.eventChoiceIndex, m_PossibleChoices);
        if (EditorGUI.EndChangeCheck())
        {
            m_Target.UpdateEventCode(m_PossibleChoices[m_Target.eventChoiceIndex]);
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif