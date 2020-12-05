/* Represents previous plays of the player
 * uses play record array to move
 */

using UnityEngine;

public class GhostCar : CarBase
{
    int m_CurrentFrameIndex = 0;

    (float angleChange, float delta)[] m_PlayRecord;

    public void SetData((float, float)[] angleChangeArray, EntranceExitPair pair)
    {
        SetEntrancePoint(pair.EntrancePoint);
        m_PlayRecord = angleChangeArray;
        m_CurrentFrameIndex = 0;
    }

    public override void Tick(float delta)
    {
        if (m_CurrentFrameIndex >= m_PlayRecord.Length) return;

        delta = m_PlayRecord[m_CurrentFrameIndex].delta;
        float angleChange = m_PlayRecord[m_CurrentFrameIndex++].angleChange;
        Move(angleChange, delta);
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
