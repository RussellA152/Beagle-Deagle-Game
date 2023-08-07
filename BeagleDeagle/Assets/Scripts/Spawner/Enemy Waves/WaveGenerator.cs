using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class WaveGenerator : MonoBehaviour
{
    public WaveBeginEvents waveBegan;

    public OffScreenSpawner offScreenSpawnerScript;

    public float difficultyFactor = 0.9f;

    public List<WaveData> waves;

    private WaveData _mCurrentWave;
    public WaveData CurrentWave { get { return _mCurrentWave; } }

    private float _mDelayFactor = 1.0f;

    private bool _wavesCompleted = false;



    IEnumerator StartWaves()
    {
        _mDelayFactor = 1.0f;

        while (!_wavesCompleted)
        {

            // for each wave...
            foreach (WaveData W in waves)
            {
                // Set the duration of the wave to be the HIGHEST duration of all mini waves
                // Ex. if we have three mini waves: 1 Bat every 2 seconds for 10 seconds, 1 Skeleton every 2 seconds for 5 seconds
                // Then the duration of the wave should be 10 seconds to accomodate for all mini waves
                W.duration = W.miniWaves.Max(v => v.waveDuration);

                // if there is currently a wave ongoing, don't start a new wave yet
                if (W != _mCurrentWave && _mCurrentWave.duration > 0)
                {
                    // wait until the current wave is finished
                    yield return new WaitForSeconds(_mCurrentWave.duration);
                }

                // Only print a message to the screen if the message isn't blank
                if (W.message != "")
                {
                    waveBegan.InvokeEvent(W.message);
                }

                // For each mini wave in that wave...
                foreach (MiniWaveData A in W.miniWaves)
                {
                    _mCurrentWave = W;
                    
                    // Start spawning those enemies for that mini wave
                    // We use a coroutine so we can have multiple mini waves concurrently spawning enemies
                    StartCoroutine(StartMiniWave(A));
                }

                //if(m_CurrentWave.downTime > 0)
                //{
                //    // after all mini waves are finished, give the player some downtime
                //    yield return new WaitForSeconds(m_CurrentWave.downTime);
                //}  

                yield return null;  // prevents crash if all delays are 0
            }
            _wavesCompleted = true;

            _mDelayFactor *= difficultyFactor;
            yield return null;  // prevents crash if all delays are 0
        }
        Debug.Log("WAVES SETUP COMPLETE!");
    }

    IEnumerator StartMiniWave(MiniWaveData A)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        GameObject enemyPrefab = A.GetEnemyPrefab();

        // While this mini wave's still has duration left
        while (elapsedTime < A.waveDuration)
        {
            // Spawn an enemy per delay (ex. 1 zombie per second)
            if (A.delayBetweenSpawn > 0)
                yield return new WaitForSeconds(A.delayBetweenSpawn * _mDelayFactor);

            // Check if there is an enemy to spawn
            if (enemyPrefab != null && A.enemiesPerSpawn > 0)
            {
                // All enemies of the same type, have the same pool key
                // For example, the basic zombie runner has a pool key of 0
                int enemyPoolKey = enemyPrefab.GetComponent<IPoolable>().PoolKey;

                for (int i = 0; i < A.enemiesPerSpawn; i++)
                {
                    // update the x & z values depending on the specific boundaries of your scene
                    Vector2 randomPosition = offScreenSpawnerScript.PickRandomLocationOnMap();

                    // fetch an enemy from the object pool and place them at the random position
                    GameObject newEnemy = ObjectPooler.Instance.GetPooledObject(enemyPoolKey);

                    newEnemy.transform.position = randomPosition;

                    // Fetch all scripts that implement IEnemyDataUpdatable (i.e Attack script, Health script, and Controller script)
                    // Pass in the EnemyData scriptable object to the newly spawned enemy
                    IEnemyDataUpdatable[] dataToUpdate = newEnemy.GetComponents<IEnemyDataUpdatable>();

                    // For all scripts (within the enemy) that need their stats to be updated...
                    // call the function that takes in a scriptable object and updates values
                    // Ex. We're telling this enemy to set their maxHealth and attackDamage stats
                    foreach (IEnemyDataUpdatable data in dataToUpdate)
                    {
                        data.UpdateScriptableObject(A.GetEnemyData());
                    }

                    newEnemy.SetActive(true);
                }
            }
            // Update elapsed time
            elapsedTime = Time.time - startTime;
        }

    }

    void Start()
    {
        // Set the current wave to the first wave
        _mCurrentWave = waves[0];

        StartCoroutine(StartWaves());

        
    }
}
