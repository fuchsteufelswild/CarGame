using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCar : CarBase
{
    int m_CurrentFrameIndex = 0;
    float[] m_AngleChanges;
    
    public void SetData(float[] angleChangeArray, EntranceExitPair pair)
    {
        SetEntrancePoint(pair.EntrancePoint);
        m_AngleChanges = angleChangeArray;
        m_CurrentFrameIndex = 0;
    }

    public override void Tick(float delta)
    {
        if (m_CurrentFrameIndex >= m_AngleChanges.Length) return;

        float angleChange = m_AngleChanges[m_CurrentFrameIndex++];
        Move(angleChange);
    }

    public override void ResetToInitialAttributes()
    {
        base.ResetToInitialAttributes();

        m_CurrentFrameIndex = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == AgentTags.PLAYER_CAR_TAG)
        {
            EventManager.NotifyEvent(MissionEvents.PLAYER_BUMP);
        }
    }
}
