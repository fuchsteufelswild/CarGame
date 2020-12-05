/* Represents player controlled car.
 * Responds to input events and records
 * movement for replay.
 */

using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : CarBase
{
    float m_AngleChange;

    List<(float, float)> m_Record;

    public (float, float)[] RecordArray =>
        m_Record.ToArray();

    protected override void Start()
    {
        base.Start();
        m_Record = new List<(float, float)>();
        m_AngleChange = 0f;
        // Register to input events
        EventManager.AddListener(InputEvents.TURN_RIGHT, () => m_AngleChange -= Time.deltaTime * m_CarRotationSpeed);
        EventManager.AddListener(InputEvents.TURN_LEFT, () => m_AngleChange += Time.deltaTime * m_CarRotationSpeed);
    }

    public override void Tick(float delta)
    {
        m_Record.Add((m_AngleChange, delta));
        Move(m_AngleChange, delta);
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
        m_Record.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == SpecialAreaTags.EXIT_POINT_TAG)
        {
            EventManager.NotifyEvent(MissionEvents.WAVE_COMPLETE_SIGNAL);
        }
    }
}
