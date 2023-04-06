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

    public GameObject prefab; // should make this a list *

    public int amount_per_spawn;

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

    public float lowestRangeX = 0.0f; // minimum spawn area on x axis
    public float highestRangeX = 50.0f; // maximum spawn area on x axis
    //public float lowestRangeZ = 50.0f; // minimum spawn area on z axis
    //public float highestRangeZ = 50.0f; // maximum spawn area on z axis
    public float lowestRangeY = 0.0f; // minimum prefab rotation on y axis
    public float highestRangeY = 50.0f; // maximum prefab rotation on z axis



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
                    if (A.prefab != null && A.amount_per_spawn > 0)
                    {
                        for (int i = 0; i < A.amount_per_spawn; i++)
                        {
                            // make this retrieve an off-screen location

                            // update the x & z values depending on the specific boundaries of your scene
                            Vector2 randomizePosition = new Vector3(Random.Range(lowestRangeX, highestRangeX), Random.Range(lowestRangeY, highestRangeY));

                            // update the y values depending on how much you want the prafabs to randomly rotate
                            //Quaternion randomizeRotation = Quaternion.Euler(0, Random.Range(lowestRangeY, highestRangeY), 0);

                            GameObject newEnemy = Instantiate(A.prefab, randomizePosition, Quaternion.identity);
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
