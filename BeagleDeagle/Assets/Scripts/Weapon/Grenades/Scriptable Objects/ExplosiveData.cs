using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewExplosive", menuName = "ScriptableObjects/Explosives")]
public class ExplosiveData : ScriptableObject
{
    [SerializeField]
    private PhysicsMaterial2D physicsMaterial; // What physics material should this grenade use? (Ex. Bouncy material)
    
    public bool hitThroughWalls;
    
    [Range(1f, 100f)] public float explosiveRadius;

    public LayerMask whatDoesExplosionHit;

    [Header("Grenade Timers"), Range(0.1f, 30f)]
    public float detonationTime; // how long until this grenade detonates?

    // What AOE does this explosive use?
    public AreaOfEffectData aoeData;

    [Header("Sounds")]
    // What sound is played when this detonates?
    public AudioClip[] explosionClips;
    [Range(0.1f, 1f)]
    public float explosiveSoundVolume;

}
