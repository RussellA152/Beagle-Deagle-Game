using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewHealthRegeneration", menuName = "ScriptableObjects/Stat Modifiers/Health Regeneration")]
public class HealthRegenData : ScriptableObject
{
    // How much health can the player regenerate up to? (ex. player can regenerate up to 50% health, but must be below 50% health)
    [Range(0.1f, 1f)] public float regenThreshold;
    
    // How much does the player regenerate?
    [Range(1f, 2500f)] public float regenRate;
}
