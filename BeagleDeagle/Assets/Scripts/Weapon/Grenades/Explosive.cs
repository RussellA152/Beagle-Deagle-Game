using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Explosive<T> : MonoBehaviour, IGrenadeUpdatable where T: GrenadeData
{
    [SerializeField]
    protected GameObject sprite;

    [SerializeField]
    protected T explosiveData;
    
    [SerializeField]
    protected GameObject areaOfEffectGameObject;
    
    private int _wallLayerMask;
    

    private void Start()
    {
        _wallLayerMask = LayerMask.GetMask("Wall");
    }

    private void OnDisable()
    {
        areaOfEffectGameObject.gameObject.SetActive(false);

        sprite.SetActive(true);

        StopAllCoroutines();
    }


    // Wait some time, then activate the grenade's explosion
    // Then after some more time, disable this grenade
    public abstract IEnumerator Detonate();

    protected virtual void Explode()
    {
        // Play explosion sound

        // Screen shake
        Debug.Log("EXPLODE! " + name);
    }

    protected bool CheckObstruction(Collider2D targetCollider)
    {
        if (explosiveData.hitThroughWalls)
        {
            return false;
        }
        
        Vector3 targetPosition = targetCollider.transform.position;
        Vector3 direction = targetPosition - transform.position;
        float distance = direction.magnitude;

        // Exclude the target if there is an obstruction between the explosion source and the target
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distance, _wallLayerMask);

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

    public void UpdateScriptableObject(GrenadeData scriptableObject)
    {
        if (scriptableObject is T)
        {
            explosiveData = scriptableObject as T;
        }
        else
        {
            Debug.Log("ERROR WHEN UPDATING SCRIPTABLE OBJECT! PREFAB DID NOT UPDATE ITS SCRIPTABLE OBJECT");
        }
    }
}
