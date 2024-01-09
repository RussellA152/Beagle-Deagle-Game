using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObjects/WaveData/MiniWave")]
public abstract class MiniWaveData : ScriptableObject
{
    public string miniWaveName;
    
    [Space(30)]

    [Range(0f, 60f)]
    // How much time between each enemy spawn?
    public float delayBetweenSpawn;

    [Range(0, 100)]
    // How many enemies will spawn in each interval (ex. 2 enemies every 3 seconds)
    public int enemiesPerSpawn;

    //[Range(1f, 180f)]
    // How long does this wave last?
    //public float waveDuration;
    
    public abstract GameObject GetEnemyPrefab();
    
    public abstract EnemyData GetEnemyData();
    

}

