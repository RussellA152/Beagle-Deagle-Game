using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    private AreaOfEffectData _areaOfEffectData;

    [SerializeField]
    private CapsuleCollider2D triggerCollider;
    
    // The layer mask for walls
    private int _wallLayerMask;

    private void Start()
    {
        _wallLayerMask = LayerMask.GetMask("Wall");
    }

    private void OnDestroy()
    {
        // Resetting the size of the trigger collider when destroyed
        triggerCollider.size = new Vector2(1f, 1f);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AreaOfEffectManager.Instance.AddNewAreaOfEffect(_areaOfEffectData);
        
        // If this AOE hits what its allowed to affect
        if ((_areaOfEffectData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Target has entered AOE, add them to overlappingTargets dictionary
            AreaOfEffectManager.Instance.AddNewOverlappingTarget(_areaOfEffectData, collision.gameObject);
            
        }
            
            

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((_areaOfEffectData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Target has left AOE, so remove them from overlappingTarget dictionary
            // Also, they are no longer affected by their AOE, so remove them from affectedTargets dictionary (* Not exactly the case for DOT's *)
            // DOT's will also remove from affectedTargets if they had to reapply their DOT
            AreaOfEffectManager.Instance.RemoveTargetFromOverlappingDictionary(_areaOfEffectData, collision.gameObject);
            AreaOfEffectManager.Instance.RemoveTargetFromAffected(_areaOfEffectData, collision.gameObject);
            
            // If the target is not standing in any AOE at all, then call OnAreaExit()
            if(!AreaOfEffectManager.Instance.IsTargetOverlappingAreaOfEffect(_areaOfEffectData, collision.gameObject))
                if(_areaOfEffectData.removeEffectOnTriggerExit)
                    _areaOfEffectData.RemoveEffectFromEnemies(collision.gameObject);
            
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If this AOE hits what its allowed to affect
        if ((_areaOfEffectData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            // If the target is not obstructed by a wall, and they are not already affected by the AOE's ability (smoke or radiation), then call OnAreaStay()
            if (!CheckObstruction(transform.position, collision.gameObject) && AreaOfEffectManager.Instance.CheckIfTargetCanBeAffected(_areaOfEffectData, collision.gameObject))
            {
                AreaOfEffectManager.Instance.TryAddAffectedTarget(_areaOfEffectData, collision.gameObject);
                _areaOfEffectData.AddEffectOnEnemies(collision.gameObject);
            }
                
            
        }
    }
    ///-///////////////////////////////////////////////////////////
    /// Shoot a raycast beginning from the center of the AOE towards the target
    /// If there is wall between the AOE and target, then do not apply any effects to the target (Don't hit through walls)
    ///
    private bool CheckObstruction(Vector2 areaSource, GameObject target)
    {
        if (_areaOfEffectData.hitThroughWalls)
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

    ///-///////////////////////////////////////////////////////////
    /// Changes the AOE effect of this instance
    /// 
    public void UpdateAOEData(AreaOfEffectData scriptableObject)
    {
        _areaOfEffectData = scriptableObject;

        triggerCollider.size = new Vector2(_areaOfEffectData.areaSpreadX, _areaOfEffectData.areaSpreadY);
    }
}
