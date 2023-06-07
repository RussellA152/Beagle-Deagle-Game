using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGrenade", menuName = "ScriptableObjects/Grenade")]
public abstract class GrenadeData : ScriptableObject
{
    public LayerMask whatAreaOfEffectCollidesWith; // What should this grenade collide with (Ex. hitting and bouncing off a wall)

    //public float radius; // How big should this explosion be?

    [Header("Size of the Area of Effect")]

    [Range(0f, 100f)]
    public float areaSpreadX;
    [Range(0f, 100f)]
    public float areaSpreadY;

    [Header("Speed of Grenade")]

    [Range(0f, 100f)]
    public float throwSpeed = 15f;

    [Header("Grenade Timers")]

    [Range(0f, 100f)]
    public float duration; // how long does the explosion, or AOE linger?
    [Range(0f, 30f)]
    public float detonationTime; // how long until this grenade detonates?

    public abstract void Explode();

    public abstract void OnAreaEnter(Collider2D collision);

    public abstract void OnAreaExit(Collider2D collision);

}
