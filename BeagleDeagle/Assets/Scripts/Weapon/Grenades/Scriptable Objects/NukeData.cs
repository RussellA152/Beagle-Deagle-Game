using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewExplosive", menuName = "ScriptableObjects/Explosive/Nuclear Bomb")]
public class NukeData : ExplosiveData
{
    [Range(1f, 100f)] public float explosiveRadius;
    
    public LayerMask whatDoesExplosionHit;
    

}
