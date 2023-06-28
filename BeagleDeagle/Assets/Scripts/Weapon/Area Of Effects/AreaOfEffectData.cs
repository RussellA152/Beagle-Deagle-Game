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


    ///-///////////////////////////////////////////////////////////
    /// When the target enters the trigger collider -> Do something
    ///
    public virtual void OnAreaEnter(GameObject target)
    {
        // NOT SURE HOW TO NOT MAKE THIS EMPTY
    }

    ///-///////////////////////////////////////////////////////////
    /// When the target exits the trigger collider -> Do something
    /// In this case, we remove the target from the overlappingEnemies dictionary
    ///
    public virtual void OnAreaExit(GameObject target)
    {
        RemoveEffectFromEnemies(target);

    }

    ///-///////////////////////////////////////////////////////////
    /// Do something WHILE the target is inside of the trigger collider
    /// Usually, this is where we try to reapply a DOT to an target
    ///
    public virtual void OnAreaStay(Vector2 areaSource, GameObject target)
    {
        AddEffectOnEnemies(target);
    }

    ///-///////////////////////////////////////////////////////////
    /// Add some sort of buff or debuff (or DOT) to the target that is inside of the AOE
    ///
    protected abstract void AddEffectOnEnemies(GameObject target);

    ///-///////////////////////////////////////////////////////////
    /// Remove the applied buff or debuff from the target when they exit the AOE
    ///
    protected abstract void RemoveEffectFromEnemies(GameObject target);
    
}
