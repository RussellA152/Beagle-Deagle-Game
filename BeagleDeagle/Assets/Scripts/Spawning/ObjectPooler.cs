using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public int amountToPool;

    public GameObject objectToPool;

    public bool shouldExpand;

}


public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;

    [NonReorderable]
    public List<ObjectPoolItem> itemsToPool;

    [NonReorderable]
    public List<GameObject> pooledObjects;


    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeObjectPool();

    }

    public void InitializeObjectPool()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(int key)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            // checking if the pooled object is disabled in the inspector
            // AND if the pool key is the same as the one being passed in
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].GetComponent<IPoolable>().PoolKey == key)
            {
                Debug.Log("RETURN OBJECT!");
                return pooledObjects[i];
            }
            else
            {
                Debug.Log("FAILED TO RETURN OBJECT!");
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

}
