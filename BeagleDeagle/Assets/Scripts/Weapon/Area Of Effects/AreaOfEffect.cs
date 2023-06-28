using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    private AreaOfEffectData aoeData;

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
        AreaOfEffectManager.Instance.AddNewAreaOfEffect(aoeData);
        
        // If this bullet hits what its allowed to damage
        if ((aoeData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            AreaOfEffectManager.Instance.AddNewOverlappingTarget(aoeData, collision.gameObject);
            
            if(AreaOfEffectManager.Instance.CheckIfTargetIsOverlapping(aoeData, collision.gameObject))
                aoeData.OnAreaEnter(collision.gameObject);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // If this bullet hits what its allowed to damage
        if ((aoeData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            if (AreaOfEffectManager.Instance.RemoveOverlappingTarget(aoeData, collision.gameObject))
                aoeData.OnAreaExit(collision.gameObject);
            
            
            // Only remove AOE effect if the target is not standing in any AOE(s) at all
            // if(AreaOfEffectManager.Instance.CheckIfTargetIsNotStandingInAreaOfEffect(aoeData, collision.gameObject))
            //     aoeData.OnAreaExit(collision.gameObject);
            //
            // AreaOfEffectManager.Instance.RemoveOverlappingTarget(aoeData, collision.gameObject);

        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If this bullet hits what its allowed to damage
        if ((aoeData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            if (!CheckObstruction(transform.position, collision.gameObject) && AreaOfEffectManager.Instance.CheckIfTargetCanBeAffected(aoeData, collision.gameObject))
            {
                aoeData.OnAreaStay(transform.position, collision.gameObject);
            }
                
            
        }
    }
    ///-///////////////////////////////////////////////////////////
    /// Shoot a raycast beginning from the center of the AOE towards the target
    /// If there is wall between the AOE and target, then do not apply any effects to the target (Don't hit through walls)
    ///
    private bool CheckObstruction(Vector2 areaSource, GameObject target)
    {
        if (aoeData.hitThroughWalls)
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

    public void UpdateAOEData(AreaOfEffectData scriptableObject)
    {
        aoeData = scriptableObject;

        triggerCollider.size = new Vector2(aoeData.areaSpreadX, aoeData.areaSpreadY);
    }
}
