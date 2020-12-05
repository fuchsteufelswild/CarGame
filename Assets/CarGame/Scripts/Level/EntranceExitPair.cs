using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor; 
#endif

public class EntranceExitPair : MonoBehaviour
{
    [SerializeField] Transform m_EntrancePoint;
    [SerializeField] Transform m_ExitPoint;

    public Transform EntrancePoint => m_EntrancePoint;
    public Transform ExitPoint => m_ExitPoint;

    private void Start()
    {
        Sprite defaultArrow = Resources.Load<Sprite>("BlackArrow");
#if UNITY_EDITOR
        if (defaultArrow == null)
            Debug.LogError("BlackArrow sprite cannot be found in path");
#endif

        SpriteRenderer sr = m_EntrancePoint.GetComponent<SpriteRenderer>();
        sr.sprite = defaultArrow;

        sr = m_ExitPoint.GetComponent<SpriteRenderer>();
        sr.sprite = defaultArrow;
    }

    public void SetPoints(Transform entrance, Transform exit)
    {
        m_EntrancePoint = entrance;
        m_ExitPoint = exit;
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(EntranceExitPair))]
public class EntranceExitPairEditor : Editor
{
    SerializedProperty m_Entrance;
    SerializedProperty m_Exit;

    Transform m_EntranceTransform;
    Transform m_ExitTransform;

    float m_IdentifierOffsetMultiplier = 2f;

    private void OnEnable()
    {
        m_Entrance = serializedObject.FindProperty("m_EntrancePoint");
        m_Exit = serializedObject.FindProperty("m_ExitPoint");

        m_EntranceTransform = m_Entrance.objectReferenceValue as Transform;
        m_ExitTransform = m_Exit.objectReferenceValue as Transform;
    }

    void SetTransformGUI(Transform transform, string identifier)
    {
        serializedObject.Update();

        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        float uniScale = 1.0f;

        Handles.Label(pos + Vector3.up * m_IdentifierOffsetMultiplier, identifier);

        EditorGUI.BeginChangeCheck();

        Handles.TransformHandle(ref pos, ref rot, ref uniScale);

        if(EditorGUI.EndChangeCheck())
        {
            Undo.RegisterCompleteObjectUndo(transform, "Transform Moved");
            transform.position = pos;
            transform.rotation = Quaternion.Euler(0, 0, rot.eulerAngles.z);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        SetTransformGUI(m_EntranceTransform, "Entrance Point");
        SetTransformGUI(m_ExitTransform, "Exit Point");
    }
}
#endif