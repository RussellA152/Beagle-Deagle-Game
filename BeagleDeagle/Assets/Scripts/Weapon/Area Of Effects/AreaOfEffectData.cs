using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaOfEffectData : ScriptableObject
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

    // Key: The enemy inside of the smoke grenade's trigger collider
    // Value: The number of smoke grenade trigger colliders that the enemy is inside of
    protected Dictionary<GameObject, int> overlappingEnemies = new Dictionary<GameObject, int>();

    // A hashset of all enemies affected by the area of effect's ability (ex. smoke slow effect or radiation damage)
    protected HashSet<GameObject> affectedEnemies = new HashSet<GameObject>();

    // The layer mask for walls
    private int wallLayerMask;

    public virtual void OnEnable()
    {
        overlappingEnemies.Clear();
        affectedEnemies.Clear();

        wallLayerMask = LayerMask.GetMask("Wall");
    }

    ///-///////////////////////////////////////////////////////////
    /// When the target enters the trigger collider -> Do something
    /// In this case, we add the target to the overlappingEnemies dictionary
    ///
    public virtual void OnAreaEnter(GameObject target)
    {
        // When enemy enters the smoke grenade collider
        if (!overlappingEnemies.ContainsKey(target))
        {
            overlappingEnemies.Add(target, 0);
        }

        // Increment overlappingEnemies by 1
        overlappingEnemies[target]++;

    }

    ///-///////////////////////////////////////////////////////////
    /// When the target exits the trigger collider -> Do something
    /// In this case, we remove the taregt from the overlappingEnemies dictionary
    ///
    public virtual void OnAreaExit(GameObject target)
    {
        if (overlappingEnemies.ContainsKey(target))
        {
            overlappingEnemies[target]--;

            // When the enemy is no longer colliding with any smoke grenade trigger colliders, then remove the slow effects
            // But, only if they were affected in the first place
            if (overlappingEnemies[target] == 0)
            {
                overlappingEnemies.Remove(target);

                if (affectedEnemies.Contains(target))
                {
                    RemoveEffectFromEnemies(target);

                    affectedEnemies.Remove(target);
                }
                
            }
        }
    }

    ///-///////////////////////////////////////////////////////////
    /// Do something WHILE the target is inside of the trigger collider
    /// Usually, this is where we try to reapply a DOT to an enemy or check if there is wall between the AOE and target
    ///
    public virtual void OnAreaStay(Vector2 areaSource, GameObject target)
    {
        if (!affectedEnemies.Contains(target))
        {
            // Only apply slow effects for the first smoke grenade that the enemy walks into
            if (overlappingEnemies[target] == 1 && !CheckObstruction(areaSource, target))
            {
                affectedEnemies.Add(target);
                AddEffectOnEnemies(target);
            }
        }
    }

    ///-///////////////////////////////////////////////////////////
    /// Add some sort of buff or debuff (or DOT) to the target that is inside of the AOE
    ///
    public abstract void AddEffectOnEnemies(GameObject target);

    ///-///////////////////////////////////////////////////////////
    /// Remove the applied buff or debuff from the target when they exit the AOE
    ///
    public abstract void RemoveEffectFromEnemies(GameObject target);

    ///-///////////////////////////////////////////////////////////
    /// Shoot a raycast beginning from the center of the AOE towards the target
    /// If there is wall between the AOE and target, then do not apply any effects to the target (Don't hit through walls)
    ///
    public bool CheckObstruction(Vector2 areaSource, GameObject target)
    {
        if (hitThroughWalls)
        {
            return false;
        }
        
        // Calculate distance and direction to shoot raycast
        Vector2 targetPosition = target.transform.position;
        Vector2 direction = targetPosition - areaSource;
        float distance = direction.magnitude;

        // Exclude the target if there is an obstruction between the explosion source and the target
        RaycastHit2D hit = Physics2D.Raycast(areaSource, direction.normalized, distance, wallLayerMask);

        if (hit.collider != null)
        {
            // There is an obstruction, so don't apply this AOE's effect
            return true;
        }
        else
        {
            // If no obstruction found, allow this target to be affected by the AOE
            return false;
        }
    }
}
