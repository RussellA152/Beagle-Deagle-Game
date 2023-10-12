using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExplosiveData : ScriptableObject
{
    [SerializeField]
    private PhysicsMaterial2D physicsMaterial; // What physics material should this grenade use? (Ex. Bouncy material)
    
    public bool hitThroughWalls;

    [Header("Grenade Timers")]
    [Range(0f, 30f)]
    public float detonationTime; // how long until this grenade detonates?

    public AreaOfEffectData aoeData;

}
