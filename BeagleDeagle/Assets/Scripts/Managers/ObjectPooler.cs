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
    
    public List<ObjectPoolItem> itemsToPool;

    private Dictionary<int, ObjectPoolItem> _allUniqueItems = new Dictionary<int, ObjectPoolItem>();

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

            // Add the unique item to a dictionary
            _allUniqueItems.Add(item.poolKey, item);


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

    ///-///////////////////////////////////////////////////////////
    /// Add a certain amount of gameObject to the object pool, if its not already in the object pool
    /// 
    // public void AddNewGameObject(GameObject newPooledItem, int amountToPool)
    // {
    //     int poolKey = newPooledItem.GetComponent<IPoolable>().PoolKey;
    //
    //     if (!IsItemInPool(poolKey)) return;
    //     
    //     ObjectPoolItem newItem = new ObjectPoolItem();
    //
    //     newItem.objectToPool = newPooledItem;
    //     
    //     newItem.pooled = new List<GameObject>();
    //     // iterate through all items that we want to object pool
    //     // cache a reference to their pool key so we don't have to get component too often
    //     newItem.poolKey = poolKey;
    //
    //     // Add the unique item to a dictionary
    //     _allUniqueItems.Add(newItem.poolKey, newItem);
    //
    //
    //     for (int i = 0; i < amountToPool; i++)
    //     {
    //         // Create the new object
    //         GameObject obj = Instantiate(newItem.objectToPool);
    //
    //         obj.name = obj.name + " " + i;
    //
    //         // Set its parent to a container (if it exists)
    //         if (newItem.container != null)
    //         {
    //             obj.transform.SetParent(newItem.container);
    //         }
    //
    //         obj.SetActive(false);
    //         newItem.pooled.Add(obj);
    //     }
    // }

    public GameObject GetPooledObject(int key)
    {
        ObjectPoolItem itemRequested = null;

        // Find the object that is being requested (e.g, a bullet)
        itemRequested = _allUniqueItems[key];
        
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

    public bool IsItemInPool(int key)
    {
        if (_allUniqueItems.TryGetValue(key, out ObjectPoolItem itemRequested))
        {
            return true;
        }

        return false;
    }
}
