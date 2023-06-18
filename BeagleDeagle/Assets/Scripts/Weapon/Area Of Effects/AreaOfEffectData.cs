using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaOfEffectData : ScriptableObject
{
    public LayerMask whatAreaOfEffectCollidesWith; // What should this grenade collide with (Ex. hitting and bouncing off a wall)

    //public bool hitThroughWalls;

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

    public abstract void OnAreaEnter(Collider2D targetCollider);

    public abstract void OnAreaExit(Collider2D targetCollider);

    //public bool CheckObstruction(Vector2 areaSource, Collider2D targetCollider)
    //{
    //    if (hitThroughWalls)
    //    {
    //        return false;
    //    }

    //    Vector2 targetPosition = targetCollider.transform.position;
    //    Vector2 direction = targetPosition - areaSource;
    //    float distance = direction.magnitude;

    //    // Exclude the target if there is an obstruction between the explosion source and the target
    //    RaycastHit2D hit = Physics2D.Raycast(areaSource, direction.normalized, distance, 1 << LayerMask.NameToLayer("Wall"));

    //    if (hit.collider != null)
    //    {
    //        Debug.Log("OBSTRUCTION FOUND!");
    //        // There is an obstruction, so skip this target
    //        return true;
    //    }
    //    else
    //    {
    //        // If no obstruction found, allow this target to be affected by the explosion
    //        return false;
    //    }
    //}
}
