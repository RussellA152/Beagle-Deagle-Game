using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewAreaOfEffect", menuName = "ScriptableObjects/Area of Effect")]
public class AreaOfEffectData : ScriptableObject
{
    // What should this grenade collide with (Ex. hitting and bouncing off a wall)
    public LayerMask whatAreaOfEffectCollidesWith;

    // Should this AOE be able to hit targets through walls (typically not)
    public bool hitThroughWalls;

    // How big will the AOE's trigger collider be?
    [Header("Size of the Area of Effect")]
    [Range(0f, 100f)]
    public float areaSpreadX;
    [Range(0f, 100f)]
    public float areaSpreadY;

    public Vector3 aoeParticleSize;

}
