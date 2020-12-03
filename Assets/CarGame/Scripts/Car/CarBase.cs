using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D),
                  typeof(Rigidbody2D))]
public abstract class CarBase : MonoBehaviour
{
    protected float m_CarSpeed = 2f;
    protected float m_CarRotationSpeed = 30f;
    protected Rigidbody2D m_Rigidbody;

    // Find level object and update speeds
    private void UpdateMovementValues()
    {
        Level level = FindObjectOfType<Level>();

        m_CarSpeed = level.CarSpeed;
        m_CarRotationSpeed = level.CarRotationSpeed;
    }

    protected virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Rigidbody.gravityScale = 0;
        GetComponent<Collider2D>().isTrigger = true;
        // Register to new level loaded event that takes Level object
        EventManager.AddListener(SceneEvents.SCENE_LOAD_FINISHED, UpdateMovementValues);
    }

    public abstract void Tick(float delta);

    protected void Move(float angleChange)
    {
        transform.Rotate(Vector3.forward * angleChange);
        transform.position += transform.up * m_CarSpeed * Time.deltaTime;
    }
}
