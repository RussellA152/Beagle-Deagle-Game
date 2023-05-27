using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGrenae", menuName = "ScriptableObjects/Grenade")]
public abstract class GrenadeData : ScriptableObject
{
    public LayerMask whatGrenadeCollidesWith; // what should this grenade collide with (Ex. hitting and bouncing off a wall)

    public float throwSpeed = 15f;

    [Range(0f, 30f)]
    public float detonationTime; // how long until this grenade detonates?

    public abstract void Explode();

    public abstract void OnAreaEnter();

    public abstract void OnAreaExit();


    //public abstract void SpecialAbility(Vector2 position);

    //public abstract void OnTriggerExit(Collider2D collider);
}
