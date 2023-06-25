using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaOfEffectData : ScriptableObject
{
    public LayerMask whatAreaOfEffectCollidesWith; // What should this grenade collide with (Ex. hitting and bouncing off a wall)

    public bool hitThroughWalls;

    [Header("Size of the Area of Effect")]

    [Range(0f, 100f)]
    public float areaSpreadX;
    [Range(0f, 100f)]
    public float areaSpreadY;

    protected Dictionary<GameObject, int> overlappingEnemies = new Dictionary<GameObject, int>(); // Key: The enemy inside of the smoke grenade's trigger collider
                                                                                                  // Value: The number of smoke grenade trigger colliders that the enemy is inside of

    protected HashSet<GameObject> affectedEnemies = new HashSet<GameObject>(); // A hashset of all enemies affected by the area of effect

    public virtual void OnEnable()
    {
        overlappingEnemies.Clear();
    }

    public virtual void OnAreaEnter(Collider2D targetCollider)
    {
        // When enemy enters the smoke grenade collider
        if (!overlappingEnemies.ContainsKey(targetCollider.gameObject))
        {
            overlappingEnemies.Add(targetCollider.gameObject, 0);
        }

        // Increment overlappingEnemies by 1
        overlappingEnemies[targetCollider.gameObject]++;

    }

    public virtual void OnAreaExit(Collider2D targetCollider)
    {
        if (overlappingEnemies.ContainsKey(targetCollider.gameObject))
        {
            // Decrement overlappingEnemies by 1
            overlappingEnemies[targetCollider.gameObject]--;

            // When the enemy is no longer colliding with any smoke grenade trigger colliders, then remove the slow effects
            // This ensures that the slow effect 
            if (overlappingEnemies[targetCollider.gameObject] == 0 )
            {
                overlappingEnemies.Remove(targetCollider.gameObject);

                if (affectedEnemies.Contains(targetCollider.gameObject))
                {
                    RemoveEffectFromEnemies(targetCollider);

                    affectedEnemies.Remove(targetCollider.gameObject);
                }
                
            }
        }
    }

    public virtual void OnAreaStay(Vector2 areaSource, Collider2D targetCollider)
    {
        Debug.Log("Hashset length: " + affectedEnemies.Count);

        if (!affectedEnemies.Contains(targetCollider.gameObject) && !CheckObstruction(areaSource, targetCollider))
        {
            // Only apply slow effects for the first smoke grenade that the enemy walks into
            if (overlappingEnemies[targetCollider.gameObject] == 1)
            {
                affectedEnemies.Add(targetCollider.gameObject);
                AddEffectOnEnemies(targetCollider);
            }
        }
    }

    public abstract void AddEffectOnEnemies(Collider2D targetCollider);

    public abstract void RemoveEffectFromEnemies(Collider2D targetCollider);

    public bool CheckObstruction(Vector2 areaSource, Collider2D targetCollider)
    {
        if (hitThroughWalls)
        {
            return false;
        }

        Vector2 targetPosition = targetCollider.transform.position;
        Vector2 direction = targetPosition - areaSource;
        float distance = direction.magnitude;

        // Exclude the target if there is an obstruction between the explosion source and the target
        RaycastHit2D hit = Physics2D.Raycast(areaSource, direction.normalized, distance, 1 << LayerMask.NameToLayer("Wall"));

        Debug.Log("shoot raycast!");

        if (hit.collider != null)
        {
            Debug.Log("OBSTRUCTION FOUND!");
            // There is an obstruction, so skip this target
            return true;
        }
        else
        {
            // If no obstruction found, allow this target to be affected by the explosion
            return false;
        }
    }
}
