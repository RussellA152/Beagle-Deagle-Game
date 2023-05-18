using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using Random = UnityEngine.Random;

[Serializable]
public class WaveAction
{
    public string name;

    public float delayBetweenSpawn; // what is the time between each enemy spawn (ex. 0.5 seconds between each zombie spawn)

    public GameObject prefab; // the enemy to spawn 

    public int enemiesPerSpawn; // how many enemies will spawn at once (ex. 1 zombie at a time, or 2 at the same time)

    public float miniDuration; // how long does this mini wave occur for? (in seconds)

    public string message; // this string will be displayed on the screen when this mini wave begins


}

[Serializable]
public class Wave
{
    public string name;

    [NonReorderable]
    public List<WaveAction> actions;

    [HideInInspector]
    public float duration; // the duration of the wave is the maxmimum value in the mini waves

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

    private bool wavesCompleted = false;


    IEnumerator StartWaves()
    {
        m_DelayFactor = 1.0f;

        while (!wavesCompleted)
        {
            // for each wave...
            foreach (Wave W in waves)
            {
                // Set the duration of the wave to be the HIGHEST duration of all mini waves
                // Ex. if we have three mini waves: 1 Bat every 2 seconds for 10 seconds, 1 Skeleton every 2 seconds for 5 seconds
                // Then the duration of the wave should be 10 seconds to accomodate for all mini waves
                W.duration = W.actions.Max(v => v.miniDuration);

                // if there is currently a wave ongoing, don't start a new wave yet
                if (W != m_CurrentWave && m_CurrentWave.duration > 0)
                {
                    // wait until the current wave is finished
                    yield return new WaitForSeconds(m_CurrentWave.duration);
                }

                // for each mini wave in that wave...
                foreach (WaveAction A in W.actions)
                {
                    m_CurrentWave = W;
                    // Start spawning those enemies for that mini wave
                    // We use a coroutine so we can have multiple mini waves concurrently spawning enemies
                    StartCoroutine(StartMiniWave(A));
                }

                //if(m_CurrentWave.downTime > 0)
                //{
                //    // after all mini waves are finished, give the player some downtime
                //    yield return new WaitForSeconds(m_CurrentWave.downTime);
                //}
                wavesCompleted = true;
                yield return null;  // prevents crash if all delays are 0
            }
            
            m_DelayFactor *= difficultyFactor;
            yield return null;  // prevents crash if all delays are 0
        }
        Debug.Log("GAME DONE!");
    }

    IEnumerator StartMiniWave(WaveAction A)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        // only print a message to the screen if the message isn't blank
        if (A.message != "")
        {
            textElement.text = A.message;  // print the message to a Text Mesh Pro Element on a Canvas
        }
        // while this mini wave's still has duration left
        while (elapsedTime < A.miniDuration)
        {
            // spawn an enemy per delay (ex. 1 zombie per second)
            if (A.delayBetweenSpawn > 0)
                yield return new WaitForSeconds(A.delayBetweenSpawn * m_DelayFactor);

            // check if there is an enemy to spawn
            if (A.prefab != null && A.enemiesPerSpawn > 0)
            {
                // all enemies of the same type, have the same pool key
                // For example, the basic zombie runner has a pool key of 0
                int enemyPoolKey = A.prefab.GetComponent<IPoolable>().PoolKey;

                for (int i = 0; i < A.enemiesPerSpawn; i++)
                {
                    // update the x & z values depending on the specific boundaries of your scene
                    Vector2 randomizePosition = offScreenSpawnerScript.PickRandomLocationOnMap();

                    // fetch an enemy from the object pool and place them at the random position
                    GameObject newEnemy = ObjectPooler.instance.GetPooledObject(enemyPoolKey);
                    newEnemy.transform.position = randomizePosition;
                    newEnemy.SetActive(true);
                }
            }
            // update elapsed time
            elapsedTime = Time.time - startTime;
        }
        
    }

    void Start()
    {
        // Set the current wave to the first wave
        m_CurrentWave = waves[0];

        StartCoroutine(StartWaves());
    }
}
