using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public int amountToPool;

    public GameObject objectToPool;

    public bool shouldExpand; // Should this item be allowed to instantiate more objects if needed?

    public Transform container; 

    [NonReorderable]
    public List<GameObject> pooled; // All objects in the respective pool

    [HideInInspector]
    public int poolKey; // A cached reference of the pool key that belongs to the IPoolable gameObject

}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    [NonReorderable]
    public List<ObjectPoolItem> itemsToPool;

    private object _lockObject = new object(); // Create a private lock object for synchronization


    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeObjectPool();
    }

    public void InitializeObjectPool()
    {
        foreach (ObjectPoolItem item in itemsToPool)
        {
            item.pooled = new List<GameObject>();
            // iterate through all items that we want to object pool
            // cache a reference to their pool key so we don't have to get component too often
            item.poolKey = item.objectToPool.GetComponent<IPoolable>().PoolKey;

            for (int i = 0; i < item.amountToPool; i++)
            {
                // Create the new object
                GameObject obj = Instantiate(item.objectToPool);

                obj.name = obj.name + " " + i;

                // Set its parent to a container (if it exists)
                if (item.container != null)
                {
                    obj.transform.SetParent(item.container);
                }

                obj.SetActive(false);
                item.pooled.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(int key)
    {
        ObjectPoolItem itemRequested = null;

        // First, find the object that is being requested (e.g., a bullet)
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (key == item.poolKey)
            {
                itemRequested = item;
                break;
            }
        }
        
        // If key could not be found, show an error in the console
        if(itemRequested == null)
            Debug.LogError("That item is not in the object pooler! Key: " + key);

        lock (_lockObject) // Lock the critical section to ensure exclusive access
        {
            if (itemRequested != null)
            {
                // Iterate through all of the pooled objects for that specific item
                for (int i = 0; i < itemRequested.pooled.Count; i++)
                {
                    if (!itemRequested.pooled[i].activeInHierarchy)
                    {
                        return itemRequested.pooled[i];
                    }
                }

                if (itemRequested.shouldExpand)
                {
                    GameObject obj = Instantiate(itemRequested.objectToPool);
                    obj.SetActive(false);
                    itemRequested.pooled.Add(obj);
                    return obj;
                }
            }
            
        }
        return null;
    }
}
