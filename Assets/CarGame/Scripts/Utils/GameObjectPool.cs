using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T>
        where T : Component
{
    List<T> pool;

    Transform parentTransform;
    GameObject objectPrefab;

    public GameObjectPool(int initialPoolSize, Transform parent = null, GameObject objectPrefab = null)
    {
        parentTransform = parent;
        this.objectPrefab = objectPrefab;
        pool = new List<T>();

        for (int i = 0; i < initialPoolSize; ++i)
        {
            pool.Add(CreateNewObject());
        }
    }

    public void DeactivateAll()
    {
        for (int i = 0; i < pool.Count; ++i)
            pool[i].gameObject.SetActive(false);
    }

    public T GetItem()
    {
        for (int i = 0; i < pool.Count; ++i)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                // pool[i].gameObject.SetActive(true);
                return pool[i];
            }
        }

        T newObj = CreateNewObject();

        pool.Add(newObj);

        return newObj;
    }

    private T CreateNewObject()
    {
        T newObject = null;
        if (objectPrefab != null)
        {
            newObject = GameObject.Instantiate(objectPrefab, parentTransform).GetComponent<T>();
        }
        else
        {
            GameObject go = new GameObject();
            newObject = go.AddComponent<T>();
        }

        newObject.gameObject.SetActive(false);

        if (parentTransform != null)
            newObject.transform.SetParent(parentTransform);

        return newObject;
    }
}
