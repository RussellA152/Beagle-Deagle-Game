using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // Key: The target inside of the AOE's trigger collider
    // Value: The number of AOE trigger colliders that the target is inside of
    protected Dictionary<GameObject, int> overLappingTargets = new Dictionary<GameObject, int>();

    // A hashset of all enemies affected by the area of effect's ability (ex. smoke slow effect or radiation damage)
    protected HashSet<GameObject> affectedEnemies = new HashSet<GameObject>();

    // The layer mask for walls
    private int _wallLayerMask;

    public virtual void OnEnable()
    {
        overLappingTargets.Clear();
        affectedEnemies.Clear();
        _wallLayerMask = LayerMask.GetMask("Wall");
        //SceneManager.sceneLoaded += ResetOnSceneLoad;

    }


    ///-///////////////////////////////////////////////////////////
    /// When the target enters the trigger collider -> Do something
    /// In this case, we add the target to the overlappingEnemies dictionary
    ///
    public virtual void OnAreaEnter(GameObject target)
    {
        // When the target enters the trigger collider, add them list of overlappingEnemies
        overLappingTargets.TryAdd(target, 0);

        // Increment overlappingEnemies by 1
        overLappingTargets[target]++;

    }

    ///-///////////////////////////////////////////////////////////
    /// When the target exits the trigger collider -> Do something
    /// In this case, we remove the target from the overlappingEnemies dictionary
    ///
    public virtual void OnAreaExit(GameObject target)
    {
        if (overLappingTargets.ContainsKey(target))
        {
            overLappingTargets[target]--;

            // When the target is no longer colliding with any smoke grenade trigger colliders, then remove the slow effects
            // But, only if they were affected in the first place
            if (overLappingTargets[target] == 0)
            {
                overLappingTargets.Remove(target);

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
    /// Usually, this is where we try to reapply a DOT to an target or check if there is wall between the AOE and target
    ///
    public virtual void OnAreaStay(Vector2 areaSource, GameObject target)
    {
        if (!affectedEnemies.Contains(target))
        {
            // Only apply debuff effects for the first AOE that the target walks into
            if (overLappingTargets[target] == 1 && !CheckObstruction(areaSource, target))
            {
                affectedEnemies.Add(target);
                AddEffectOnEnemies(target);
            }
        }
    }

    ///-///////////////////////////////////////////////////////////
    /// Add some sort of buff or debuff (or DOT) to the target that is inside of the AOE
    ///
    protected abstract void AddEffectOnEnemies(GameObject target);

    ///-///////////////////////////////////////////////////////////
    /// Remove the applied buff or debuff from the target when they exit the AOE
    ///
    protected abstract void RemoveEffectFromEnemies(GameObject target);

    ///-///////////////////////////////////////////////////////////
    /// Shoot a raycast beginning from the center of the AOE towards the target
    /// If there is wall between the AOE and target, then do not apply any effects to the target (Don't hit through walls)
    ///
    private bool CheckObstruction(Vector2 areaSource, GameObject target)
    {
        if (hitThroughWalls)
            return false;

            // Calculate distance and direction to shoot raycast
        Vector2 targetPosition = target.transform.position;
        Vector2 direction = targetPosition - areaSource;
        float distance = direction.magnitude;

        // Exclude the target if there is an obstruction between the explosion source and the target
        RaycastHit2D hit = Physics2D.Raycast(areaSource, direction.normalized, distance, _wallLayerMask);

        // If true, there is an obstruction between the AOE and target
        // If false, there is not an obstruction between AOE and target, thus we can apply the effect
        return hit.collider != null;
    }
    
}
