using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public int amountToPool;

    public GameObject objectToPool;

    public bool shouldExpand; // should this item be allowed to instaniate more objects if needed?

    [NonReorderable]
    public List<GameObject> pooled;

    [HideInInspector]
    public int poolKey; // this is a cached reference of the pool key that belongs to the IPoolable gameobject

}


public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;

    [NonReorderable]
    public List<ObjectPoolItem> itemsToPool;

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

        foreach (ObjectPoolItem item in itemsToPool)
        {
            item.pooled = new List<GameObject>();
            // iterate through all items that we want to object pool
            // cache a reference to their pool key so we don't have to get component too often
            item.poolKey = item.objectToPool.GetComponent<IPoolable>().PoolKey;

            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                item.pooled.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(int key)
    {
        ObjectPoolItem itemRequested = null;

        // first, find the object that is being requested (ex. a bullet)
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (key == item.poolKey)
                itemRequested = item;
        }

        if(itemRequested != null)
        {
            // iterate through all of the pooled objects for that specific items
            for (int i = 0; i < itemRequested.pooled.Count; i++)
            {
                // fetch the first object that is disabled in the scene
                if (!itemRequested.pooled[i].activeInHierarchy)
                {
                    return itemRequested.pooled[i];
                }
            }

            // if the pool is allowed to expand, then instaniate a new item and add it to the pool
            if (itemRequested.shouldExpand)
            {
                GameObject obj = Instantiate(itemRequested.objectToPool);
                obj.SetActive(false);
                itemRequested.pooled.Add(obj);
                return obj;
            }   
        }

        return null;


        //for (int i = 0; i < pooledObjects.Count; i++)
        //{
        //    // checking if the pooled object is disabled in the inspector
        //    // AND if the pool key is the same as the one being passed in
        //    // This iterates through the entire pooled objects list....


        //    if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].GetComponent<IPoolable>().PoolKey == key)
        //    {
        //        return pooledObjects[i];
        //    }
        //    else
        //    {
        //        Debug.Log("FAILED TO RETURN OBJECT");
        //    }
        //}
        //foreach (ObjectPoolItem item in itemsToPool)
        //{
        //    if (item.objectToPool.GetComponent<IPoolable>().PoolKey == key)
        //    {
        //        if (item.shouldExpand)
        //        {
        //            GameObject obj = (GameObject)Instantiate(item.objectToPool);
        //            obj.SetActive(false);
        //            //pooledObjects.Add(obj);
        //            return obj;
        //        }
        //    }
        //}
        //return null;
    }
}
