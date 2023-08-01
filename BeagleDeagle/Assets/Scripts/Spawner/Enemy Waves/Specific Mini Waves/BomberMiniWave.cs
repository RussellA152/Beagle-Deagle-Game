using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMiniWave", menuName = "ScriptableObjects/MiniWaveData/Bomber Enemies")]
public class BomberMiniWave : MiniWaveData
{
    [RestrictedPrefab(typeof(AIController<BomberEnemyData>))]
    public GameObject enemyPrefab;
    
    public BomberEnemyData enemyData;
    
    public override GameObject GetEnemyPrefab()
    {
        return enemyPrefab;
    }

    public override EnemyData GetEnemyData()
    {
        return enemyData;
    }
}
