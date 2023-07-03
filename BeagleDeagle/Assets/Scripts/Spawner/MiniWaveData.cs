using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObjects/WaveData/MiniWave")]
public class MiniWaveData : ScriptableObject
{
    public string name;

    public float delayBetweenSpawn;

    [RestrictedPrefab(typeof(IEnemyDataUpdatable))]
    public GameObject enemyPrefab;

    public EnemyData enemyData;

    public int enemiesPerSpawn;

    public float miniDuration;

}

