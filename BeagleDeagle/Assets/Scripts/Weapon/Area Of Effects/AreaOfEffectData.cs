using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaOfEffectData : ScriptableObject
{
    public LayerMask whatAreaOfEffectCollidesWith; // What should this grenade collide with (Ex. hitting and bouncing off a wall)

    [Header("Size of the Area of Effect")]

    [Range(0f, 100f)]
    public float areaSpreadX;
    [Range(0f, 100f)]
    public float areaSpreadY;

    protected Dictionary<GameObject, int> overlappingEnemies = new Dictionary<GameObject, int>(); // Key: The enemy inside of the smoke grenade's trigger collider
                                                                                                // Value: The number of smoke grenade trigger colliders that the enemy is inside of

    public virtual void OnEnable()
    {
        overlappingEnemies.Clear();
    }

    public abstract void OnAreaEnter(Collider2D collision);

    public abstract void OnAreaExit(Collider2D collision);
}
