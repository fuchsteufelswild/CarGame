/* A component that can be used to let obstacles
 * go back and forth between two designated points.
 * Two handles are provided for setting end points.
 * Also speed, sensitivity, and alignment settings
 * can be set in the inspector.
 */

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[RequireComponent(typeof(Rigidbody2D))]
public class MovingObstacle : ObstacleBase
{
    [SerializeField] [HideInInspector] Vector3 m_MinMovePosition = Vector3.zero;
    [SerializeField] [HideInInspector] Vector3 m_MaxMovePosition = Vector3.zero;

    [Header("Alignment Attributes")]
    [SerializeField] bool m_AlignHorizontally = false;
    [SerializeField] bool m_AlignVertically = false;

    [Header("Movement Attributes")]
    [SerializeField] [Range(0, 5)] float m_ObjectSpeed = 0.5f;
    [SerializeField] float m_DistanceSensivity = 1f;

    public Transform ObjectTransform => m_ObjectTransform;

    bool m_MovingToMinPosition = true;
    
    Transform m_ObjectTransform = null;
    Rigidbody2D m_Rigidbody = null;
    protected override void Start()
    {
        base.Start();

        m_ObjectTransform = transform;
        m_Rigidbody = GetComponent<Rigidbody2D>();

#if UNITY_EDITOR
        if (m_Rigidbody == null)
        {
            Debug.LogError("RIGIDBODY MISSING");
        }
#endif

        m_Rigidbody.gravityScale = 0;
        m_Rigidbody.isKinematic = true;
    }

    void MoveToTarget(Vector3 targetPosition)
    {
        m_ObjectTransform.position = Vector3.Lerp(m_ObjectTransform.position, targetPosition, m_ObjectSpeed * Time.deltaTime);

        if (Vector3.Distance(m_ObjectTransform.position, targetPosition) <= m_DistanceSensivity)
            m_MovingToMinPosition = !m_MovingToMinPosition;
    }

    void UpdatePosition()
    {
        if (m_MovingToMinPosition)
            MoveToTarget(m_MinMovePosition);
        else
            MoveToTarget(m_MaxMovePosition);
    }

    private void FixedUpdate() => UpdatePosition();
}


#if UNITY_EDITOR
[CustomEditor(typeof(MovingObstacle))]
public class MovingObstacleEditor : Editor
{
    SerializedProperty m_MinMovePositionVector;
    SerializedProperty m_MaxMovePositionVector;

    SerializedProperty m_AlignHorizontal;
    SerializedProperty m_AlignVertical;

    MovingObstacle m_Target = null;

    float m_IdentifierOffsetMultiplier = 2.0f;

    private void OnEnable()
    {
        m_Target = target as MovingObstacle;

        m_MinMovePositionVector = serializedObject.FindProperty("m_MinMovePosition");
        m_MaxMovePositionVector = serializedObject.FindProperty("m_MaxMovePosition");

        m_AlignHorizontal = serializedObject.FindProperty("m_AlignHorizontally");
        m_AlignVertical = serializedObject.FindProperty("m_AlignVertically");
    }


    private void OnSceneGUI()
    {
        serializedObject.Update();

        Handles.Label(m_MinMovePositionVector.vector3Value + Vector3.up * m_IdentifierOffsetMultiplier, "Min Bound");
        Handles.Label(m_MaxMovePositionVector.vector3Value + Vector3.up * m_IdentifierOffsetMultiplier, "Max Bound");

        m_MinMovePositionVector.vector3Value = Handles.PositionHandle(m_MinMovePositionVector.vector3Value, m_Target.transform.rotation);
        m_MaxMovePositionVector.vector3Value = Handles.PositionHandle(m_MaxMovePositionVector.vector3Value, m_Target.transform.rotation);

        Vector3 minVector = m_MinMovePositionVector.vector3Value;
        Vector3 maxVector = m_MaxMovePositionVector.vector3Value;

        m_MinMovePositionVector.vector3Value = new Vector3(m_AlignVertical.boolValue ? m_Target.transform.position.x : minVector.x, 
                                                           m_AlignHorizontal.boolValue ? m_Target.transform.position.y : minVector.y, 
                                                           m_Target.transform.position.z);

        m_MaxMovePositionVector.vector3Value = new Vector3(m_AlignVertical.boolValue ? m_Target.transform.position.x : maxVector.x, 
                                                           m_AlignHorizontal.boolValue ? m_Target.transform.position.y : maxVector.y, 
                                                           m_Target.transform.position.z);

        if (!Application.isPlaying)
        {
            m_Target.transform.position = (m_MinMovePositionVector.vector3Value + m_MaxMovePositionVector.vector3Value) / 2.0f;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif

