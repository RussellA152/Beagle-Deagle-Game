using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Explosive<T> : MonoBehaviour, IExplosiveUpdatable where T: ExplosiveData
{
    [SerializeField]
    protected GameObject sprite;

    [SerializeField]
    protected T explosiveData;
    
    [SerializeField]
    protected GameObject areaOfEffectGameObject;
    
    private int _wallLayerMask;

    protected float Damage;

    protected float Duration;


    private void Start()
    {
        _wallLayerMask = LayerMask.GetMask("Wall");
    }

    private void OnDisable()
    {
        if(areaOfEffectGameObject != null)
            areaOfEffectGameObject.gameObject.SetActive(false);

        sprite.SetActive(true);

        StopAllCoroutines();
    }

    public abstract void Activate(Vector2 aimDirection);

    
    // Wait some time, then activate the grenade's explosion
    // Then after some more time, disable this grenade
    public abstract IEnumerator Detonate();

    public virtual void Explode()
    {
        // Play explosion sound

        // Screen shake
        Debug.Log("EXPLODE! " + name);
    }

    public void SetDamage(float explosiveDamage)
    {
        Damage = explosiveDamage;
    }

    public void SetDuration(float explosiveDuration)
    {
        Duration = explosiveDuration;
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

    public void UpdateScriptableObject(ExplosiveData scriptableObject)
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
