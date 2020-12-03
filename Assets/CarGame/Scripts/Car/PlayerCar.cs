using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : CarBase
{
    float m_AngleChange;

    protected override void Start()
    {
        base.Start();
        m_AngleChange = 0f;
        // Register to input events
        EventManager.AddListener(InputEvents.TURN_RIGHT, () => m_AngleChange -= Time.deltaTime * m_CarRotationSpeed);
        EventManager.AddListener(InputEvents.TURN_LEFT, () => m_AngleChange += Time.deltaTime * m_CarRotationSpeed);
    }

    public override void Tick(float delta)
    {
        Move(m_AngleChange);
        m_AngleChange = 0;
    }

    private void Update()
    {
        Tick(Time.deltaTime);
    }
}
