using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectManager : ManagerBase,
                             IGameManager
{
    ManagerStatus IGameManager.GetStatus() => base.GetStatus();

    bool IGameManager.IsReady() => base.IsReady();

    object IGameManager.GetData() => null;

    IEnumerator IGameManager.Init()
    {
        m_Status = ManagerStatus.LOADING;
        
        Debug.Log("Object Manager Started");
        
        yield return null;

        m_Status = ManagerStatus.STARTED;
    }

    void IGameManager.UpdateData(object data)
    {
        if (data == null) return;
    }

}
