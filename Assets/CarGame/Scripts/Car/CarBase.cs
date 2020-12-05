/* Base car attributes and functionality.
 */ 

using UnityEngine;

[RequireComponent(typeof(Collider2D),
                  typeof(Rigidbody2D))]
public abstract class CarBase : MonoBehaviour
{
    protected float m_CarSpeed = 2f;
    protected float m_CarRotationSpeed = 30f;
    protected Rigidbody2D m_Rigidbody;

    Transform m_EntrancePoint;

    protected Vector3 StartPoint =>
        m_EntrancePoint.transform.position;
    protected Quaternion StartRotation =>
        m_EntrancePoint.transform.rotation;

    protected void SetEntrancePoint(Transform entrancePoint) =>
        m_EntrancePoint = entrancePoint;

    // Put car back to its starting position
    private void ResetToEntrance()
    {
        transform.position = m_EntrancePoint.position;
        transform.rotation = m_EntrancePoint.rotation;
    }

    public virtual void ResetToInitialAttributes()
    {
        ResetToEntrance();
    }

    public void UpdateParams(Level level)
    {
        m_CarSpeed = level.CarSpeed;
        m_CarRotationSpeed = level.CarRotationSpeed;
    }

    // Find level object and update speeds
    private void UpdateMovementValues()
    {
        Level level = FindObjectOfType<Level>();

        UpdateParams(level);
    }

    protected virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Rigidbody.gravityScale = 0;
        GetComponent<Collider2D>().isTrigger = true;
        
        EventManager.AddListener(SceneEvents.SCENE_LOAD_FINISHED, UpdateMovementValues);
    }

    public abstract void Tick(float delta);

    protected void Move(float angleChange, float delta)
    {
        if(!Managers.MissionManager.IsPaused)
            transform.Rotate(Vector3.forward * angleChange);

        transform.position += transform.up * m_CarSpeed * delta;
    }
}
