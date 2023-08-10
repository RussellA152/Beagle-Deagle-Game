using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Explosive<T> : MonoBehaviour, IExplosiveUpdatable where T: ExplosiveData
{
    protected T ExplosiveData;
    
    [SerializeField] protected GameObject sprite;

    protected AreaOfEffect AreaOfEffectScript;

    protected CheckObstruction ObstructionScript;
    
    protected int WallLayerMask;

    protected float Damage;

    protected float Duration;

    protected virtual void Awake()
    {
        ObstructionScript = GetComponentInParent<CheckObstruction>();
        AreaOfEffectScript =
            GetComponentInChildren<AreaOfEffect>();
    }

    private void Start()
    {
        WallLayerMask = LayerMask.GetMask("Wall");
    }

    private void OnDisable()
    {
        if(AreaOfEffectScript != null)
            AreaOfEffectScript.gameObject.SetActive(false);

        sprite.SetActive(true);

        StopAllCoroutines();
    }

    public virtual void Activate(Vector2 aimDirection)
    {
        AreaOfEffectScript.UpdateAOEData(ExplosiveData.aoeData);
    }

    
    // Wait some time, then activate the grenade's explosion
    // Then after some more time, disable this grenade
    public abstract IEnumerator Detonate();

    public virtual void Explode()
    {
        // Play explosion sound

        // Screen shake
    }

    public void SetDamage(float explosiveDamage)
    {
        Damage = explosiveDamage;
    }

    public void SetDuration(float explosiveDuration)
    {
        Duration = explosiveDuration;
    }
    

    public void UpdateScriptableObject(ExplosiveData scriptableObject)
    {
        if (scriptableObject is T)
        {
            ExplosiveData = scriptableObject as T;
        }
        else
        {
            Debug.Log("ERROR WHEN UPDATING SCRIPTABLE OBJECT! PREFAB DID NOT UPDATE ITS SCRIPTABLE OBJECT");
        }
    }
}
