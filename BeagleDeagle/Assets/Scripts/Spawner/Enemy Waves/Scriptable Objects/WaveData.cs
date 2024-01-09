using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

///-///////////////////////////////////////////////////////////
/// We can use ScriptableObjects for data about our weapons.
/// This is useful for our weapon upgrades since we can specify new damage or ammo values for the upgraded version.
/// 
[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObjects/WaveData/Wave")]
public class WaveData : ScriptableObject
{
    public string waveName;

    [Range(1f, 300f)]
    public float duration;

    [TextArea(minLines: 1, maxLines: 4)]
    public string message; // This string will be displayed on the screen when this mini wave begins
    
    public List<MiniWaveData> miniWaves;

    //[HideInInspector]
    //public float duration; // The duration of the wave is the maxmimum value in the mini waves
    
}


