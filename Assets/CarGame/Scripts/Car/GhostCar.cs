using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCar : CarBase
{
    int m_CurrentFrameIndex = 0;
    float[] m_AngleChanges;
    
    public void SetParams(float[] angleChangeArray)
    {
        m_AngleChanges = angleChangeArray;
        m_CurrentFrameIndex = 0;
    }

    public override void Tick(float delta)
    {
        float angleChange = m_AngleChanges[m_CurrentFrameIndex++];
        Move(angleChange);
    }
}
