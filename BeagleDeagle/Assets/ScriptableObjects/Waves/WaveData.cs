using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using Random = UnityEngine.Random;

// We can use ScriptableObjects for data about our weapons.
// This is useful for our weapon upgrades since we can specify new damage or ammo values for the upgraded version
[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObjects/WaveData/Wave")]
public class WaveData : ScriptableObject
{
    public string name;

    public string message; // this string will be displayed on the screen when this mini wave begins

    [NonReorderable]
    public List<MiniWave> miniWaves;

    [HideInInspector]
    public float duration; // the duration of the wave is the maxmimum value in the mini waves
}

[Serializable]
public class MiniWave
{
    public string name;

    public float delayBetweenSpawn; // what is the time between each enemy spawn (ex. 0.5 seconds between each zombie spawn)

    public GameObject prefab; // the enemy to spawn

    public EnemyData enemyData;

    public int enemiesPerSpawn; // how many enemies will spawn at once (ex. 1 zombie at a time, or 2 at the same time)

    public float miniDuration; // how long does this mini wave occur for? (in seconds)

}

