using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGrenade", menuName = "ScriptableObjects/Grenade")]
public abstract class GrenadeData : ScriptableObject
{
    [SerializeField]
    private PhysicsMaterial2D physicsMaterial; // What physics material should this grenade use? (Ex. Bouncy material)

    //public AreaOfEffectData aoeData;

    public bool hitThroughWalls;

    [Header("Grenade Timers")]
    [Range(0f, 30f)]
    public float detonationTime; // how long until this grenade detonates?


    public abstract float GetDamage();
    
    public abstract float GetDuration();

}
