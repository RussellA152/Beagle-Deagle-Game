using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObjects/WaveData/ProjectileEnemyWave")]
public class ProjectileWaveData : WaveData
{
    public List<ProjectileMiniWaveData> miniWaves;
    
    public override List<MiniWaveData> GetMiniWaveData()
    {
        List<MiniWaveData> nMiniWaves = new List<MiniWaveData>();
        
        foreach (ProjectileMiniWaveData data in miniWaves)
        {
            nMiniWaves.Add(data);
        }
        
        return nMiniWaves;
    }
}
