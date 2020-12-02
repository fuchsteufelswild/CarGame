using UnityEngine;

public class ManagerBase : MonoBehaviour
{
    protected ManagerStatus m_Status = ManagerStatus.NOT_INIT;

    protected ManagerStatus GetStatus() => m_Status;
    protected bool IsReady() => m_Status == ManagerStatus.STARTED;
}
