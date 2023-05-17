using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Random = UnityEngine.Random;

[Serializable]
public class WaveAction
{

    public string name;

    public float delay;

    public GameObject prefab;

    private IPoolable prefabPoolKey; // the pool key of this prefab (we generate enemies which are poolable game objects)

    public int amountToSpawn;

    public string message;
}

[Serializable]
public class Wave
{
    public string name;

    [NonReorderable]
    public List<WaveAction> actions;
}



public class WaveGenerator : MonoBehaviour
{
    public OffScreenSpawner offScreenSpawnerScript;

    public TMP_Text textElement; // printing the messages on Text Mesh Pro Element

    public float difficultyFactor = 0.9f;

    [NonReorderable]
    public List<Wave> waves;

    private Wave m_CurrentWave;
    public Wave CurrentWave { get { return m_CurrentWave; } }
    private float m_DelayFactor = 1.0f;


    IEnumerator SpawnLoop()
    {
        m_DelayFactor = 1.0f;
        while (true)
        {
            foreach (Wave W in waves)
            {
                m_CurrentWave = W;
                foreach (WaveAction A in W.actions)
                {
                    if (A.delay > 0)
                        yield return new WaitForSeconds(A.delay * m_DelayFactor);
                    if (A.message != "")
                    {
                        textElement.text = A.message;  // print the message to a Text Mesh Pro Element on a Canvas
                    }
                    if (A.prefab != null && A.amountToSpawn > 0)
                    {
                        // all enemies of the same type, have the same pool key
                        // For example, the basic zombie runner has a pool key of 0
                        int enemyPoolKey = A.prefab.GetComponent<IPoolable>().PoolKey;

                        for (int i = 0; i < A.amountToSpawn; i++)
                        {
                            // update the x & z values depending on the specific boundaries of your scene
                            Vector2 randomizePosition = offScreenSpawnerScript.PickRandomLocationOnMap();

                            // fetch an enemy from the object pool and place them at the random position
                            GameObject newEnemy = ObjectPooler.instance.GetPooledObject(enemyPoolKey);
                            newEnemy.transform.position = randomizePosition;

                            newEnemy.SetActive(true);
                            //GameObject newEnemy = Instantiate(A.prefab, randomizePosition, Quaternion.identity);
                        }
                    }
                }
                yield return null;  // prevents crash if all delays are 0
            }
            m_DelayFactor *= difficultyFactor;
            yield return null;  // prevents crash if all delays are 0
        }
    }
    void Start()
    {
        StartCoroutine(SpawnLoop());
    }
}


//[System.Serializable]
//public class WaveAction
//{
//    public string name;
//    public float delay;
//    public Transform prefab;
//    public int spawnCount;
//    public string message;
//}

//[System.Serializable]
//public class Wave
//{
//    public string name;
//    public List<WaveAction> actions;
//}



//public class WaveGenerator : MonoBehaviour
//{
//    public float difficultyFactor = 0.9f;
//    public List<Wave> waves;
//    private Wave m_CurrentWave;
//    public Wave CurrentWave { get { return m_CurrentWave; } }
//    private float m_DelayFactor = 1.0f;

//    IEnumerator SpawnLoop()
//    {
//        m_DelayFactor = 1.0f;
//        while (true)
//        {
//            foreach (Wave W in waves)
//            {
//                m_CurrentWave = W;
//                foreach (WaveAction A in W.actions)
//                {
//                    if (A.delay > 0)
//                        yield return new WaitForSeconds(A.delay * m_DelayFactor);
//                    if (A.message != "")
//                    {
//                        // TODO: print ingame message
//                    }
//                    if (A.prefab != null && A.spawnCount > 0)
//                    {
//                        for (int i = 0; i < A.spawnCount; i++)
//                        {
//                            // TODO: instantiate A.prefab
//                        }
//                    }
//                }
//                yield return null;  // prevents crash if all delays are 0
//            }
//            m_DelayFactor *= difficultyFactor;
//            yield return null;  // prevents crash if all delays are 0
//        }
//    }
//    void Start()
//    {
//        StartCoroutine(SpawnLoop());
//    }

//}
