using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObjects/WaveData/MiniWave")]
public class MiniWaveData : ScriptableObject
{
    public string name;

    public float delayBetweenSpawn;

    public GameObject prefab;

    public EnemyData enemyData;

    public int enemiesPerSpawn;

    public float miniDuration;

}

