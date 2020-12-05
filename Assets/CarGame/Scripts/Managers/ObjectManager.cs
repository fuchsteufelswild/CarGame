/* Responsible for keeping track of object initialization
 * and object pools.
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectManager : ManagerBase,
                             IGameManager
{
    [SerializeField] int m_InitialCarPoolSize = 7;
    [SerializeField] GhostCar m_GhostCarPrefab;

    GameObjectPool<GhostCar> m_GhostCarPool;

    public GhostCar AvailableGhostCar =>
        m_GhostCarPool.GetItem();

    ManagerStatus IGameManager.GetStatus() => base.GetStatus();

    bool IGameManager.IsReady() => base.IsReady();

    object IGameManager.GetData() => null;

    IEnumerator IGameManager.Init()
    {
        m_Status = ManagerStatus.LOADING;

        GameObject ghostContainer = GameObject.Find("Ghosts");
        if(ghostContainer == null)
            ghostContainer = new GameObject("Ghosts");

        m_GhostCarPool = new GameObjectPool<GhostCar>(m_InitialCarPoolSize, ghostContainer.transform, m_GhostCarPrefab.gameObject);

        Debug.Log("Object Manager Started");
        
        yield return null;

        m_Status = ManagerStatus.STARTED;
    }

    void IGameManager.UpdateData(object data)
    {
        if (data == null) return;
    }

}
