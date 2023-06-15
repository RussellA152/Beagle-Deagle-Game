using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGrenade", menuName = "ScriptableObjects/Grenade")]
public abstract class GrenadeData : ScriptableObject
{
    [SerializeField]
    private PhysicsMaterial2D physicsMaterial; // What physics material should this grenade use? (Ex. Bouncy material)

    public AreaOfEffectData aoeData;

    [Header("Speed of Grenade")]

    [Range(0f, 100f)]
    public float throwSpeed = 15f;

    [Header("Grenade Timers")]
    [Range(0f, 30f)]
    public float detonationTime; // how long until this grenade detonates?

    public abstract void Explode(Vector2 position);


    public abstract float GetDuration();

}
