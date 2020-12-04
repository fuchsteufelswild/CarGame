using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : CarBase
{
    float m_AngleChange;
    List<float> m_AngleChanges;

    public float[] AngleChangeArray =>
        m_AngleChanges.ToArray();

    protected override void Start()
    {
        base.Start();
        m_AngleChanges = new List<float>();
        m_AngleChange = 0f;
        // Register to input events
        EventManager.AddListener(InputEvents.TURN_RIGHT, () => m_AngleChange -= Time.deltaTime * m_CarRotationSpeed);
        EventManager.AddListener(InputEvents.TURN_LEFT, () => m_AngleChange += Time.deltaTime * m_CarRotationSpeed);
    }

    public override void Tick(float delta)
    {
        m_AngleChanges.Add(m_AngleChange);
        Move(m_AngleChange);
        m_AngleChange = 0;
    }

    public void SetData(EntranceExitPair pair)
    {
        SetEntrancePoint(pair.EntrancePoint);
    }

    public override void ResetToInitialAttributes()
    {
        base.ResetToInitialAttributes();
        m_AngleChange = 0f;
        m_AngleChanges.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == SpecialAreaTags.EXIT_POINT_TAG)
        {
            EventManager.NotifyEvent(MissionEvents.WAVE_COMPLETE_SIGNAL);
        }
    }
}
