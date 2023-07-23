using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaOfEffect : MonoBehaviour
{
    [SerializeField]
    protected AreaOfEffectData areaOfEffectData;

    //private AreaOfEffectData _previousAreaOfEffectData;

    private Outliner _outliner;
    
    private CapsuleCollider2D _triggerCollider;
    
    
    // The layer mask for walls
    private int _wallLayerMask;

    [SerializeField] 
    private UnityEvent<GameObject> onAreaEnter;
    
    [SerializeField] 
    private UnityEvent<GameObject> onAreaStay;
    
    [SerializeField] 
    private UnityEvent<GameObject> onAreaExit;


    private void Awake()
    {
        _triggerCollider = GetComponent<CapsuleCollider2D>();

        _outliner = GetComponent<Outliner>();
        
    }

    private void Start()
    {
        _wallLayerMask = LayerMask.GetMask("Wall");

    }

    private void OnEnable()
    {
        AreaOfEffectManager.Instance.AddNewAreaOfEffect(areaOfEffectData);
        
        UpdateAOEData(areaOfEffectData);
        
        
    }

    private void OnDestroy()
    {
        // Resetting the size of the trigger collider when destroyed
        _triggerCollider.size = new Vector2(1f, 1f);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If this AOE hits what its allowed to affect
        if ((areaOfEffectData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Target has entered AOE, add them to overlappingTargets dictionary
            AreaOfEffectManager.Instance.AddNewOverlappingTarget(areaOfEffectData, collision.gameObject);
            
            onAreaEnter.Invoke(collision.gameObject);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((areaOfEffectData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Target has left AOE, so remove them from overlappingTarget dictionary
            // Also, they are no longer affected by their AOE, so remove them from affectedTargets dictionary (* Not exactly the case for DOT's *)
            // DOT's will also remove from affectedTargets if they had to reapply their DOT
            AreaOfEffectManager.Instance.RemoveOverlappingTarget(areaOfEffectData, collision.gameObject);

            // If the target is not standing in any AOE at all, then call OnAreaExit()
            if (!AreaOfEffectManager.Instance.IsTargetOverlappingAreaOfEffect(areaOfEffectData, collision.gameObject) && AreaOfEffectManager.Instance.CheckIfTargetIsAffected(areaOfEffectData, collision.gameObject))
            {
                // Only remove effect if this type of AOE is required to
                
                onAreaExit.Invoke(collision.gameObject);

                AreaOfEffectManager.Instance.RemoveTargetFromAffectedHashSet(areaOfEffectData, collision.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If this AOE hits what its allowed to affect
        if ((areaOfEffectData.whatAreaOfEffectCollidesWith.value & (1 << collision.gameObject.layer)) > 0)
        {
            // If the target is not obstructed by a wall, and they are not already affected by the AOE's ability (smoke or radiation), then call OnAreaStay()
            if (!CheckObstruction(transform.position, collision.gameObject) && !AreaOfEffectManager.Instance.CheckIfTargetIsAffected(areaOfEffectData, collision.gameObject))
            {
                AreaOfEffectManager.Instance.TryAddAffectedTarget(areaOfEffectData, collision.gameObject);

                onAreaStay.Invoke(collision.gameObject);
            }
        }
    }
    

    ///-///////////////////////////////////////////////////////////
    /// Shoot a raycast beginning from the center of the AOE towards the target
    /// If there is wall between the AOE and target, then do not apply any effects to the target (Don't hit through walls)
    ///
    private bool CheckObstruction(Vector2 areaSource, GameObject target)
    {
        if (areaOfEffectData.hitThroughWalls)
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
        //Debug.Log("WTF:   " + scriptableObject + "     prev: " + _previousAreaOfEffectData);
        
        // Only update scriptable object if its a new set of values
        //if (scriptableObject == _previousAreaOfEffectData) return;

        areaOfEffectData = scriptableObject;
        
        //_previousAreaOfEffectData = areaOfEffectData;
        
        //transform.localScale = new Vector2(areaOfEffectData.areaSpreadX, areaOfEffectData.areaSpreadY);

        _triggerCollider.size = new Vector2(areaOfEffectData.areaSpreadX, areaOfEffectData.areaSpreadY); 
        _outliner.UpdateOutlinerSize();
        
    
        Debug.Log("Updated AOE scriptable object!");


    }
}
