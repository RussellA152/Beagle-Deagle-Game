using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeHandler : MonoBehaviour, IDamageOverTimeHandler, IPlayStatusParticle
{
    // The health script of this target
    private IHealth _healthScript;

    [Header("All Damage Over Time Effects Afflicted With")]
    // All damage over time effects that have been applied to this target
    [SerializeField, NonReorderable]
    private List<DamageOverTime> damageOverTimeEffects = new List<DamageOverTime>();

    private void OnEnable()
    {
        _healthScript ??= GetComponent<IHealth>();
    }

    public void RevertAllModifiers()
    {
        foreach (DamageOverTime dot in damageOverTimeEffects)
        {
            dot.isActive = false;

            damageOverTimeEffects.Remove(dot);
        }
    }

    ///-///////////////////////////////////////////////////////////
    /// Add a new DamageOverTime effect to this target,
    /// then start a coroutine that will wait some time to remove it.
    public void AddDamageOverTime(DamageOverTime dotToAdd)
    {
        damageOverTimeEffects.Add(dotToAdd);

        StartCoroutine(TakeDamageOverTime(dotToAdd));

    }

    ///-///////////////////////////////////////////////////////////
    /// Remove the DamageOverTime effect from this target, then check 
    /// if it needs to be reapplied.
    public void RemoveDamageOverTime(DamageOverTime dotToRemove)
    {
        damageOverTimeEffects.Remove(dotToRemove);

        // Check if we need to reapply the DOT
        ReapplyDot(dotToRemove);

        
    }

    ///-///////////////////////////////////////////////////////////
    /// Make the target take damage (or heal) every "tickInterval" seconds for a 
    /// "tick" amount of times.
    public IEnumerator TakeDamageOverTime(DamageOverTime dot)
    {
        dot.isActive = true;
        
        float ticks = dot.ticks;

        // Find particle effect associated with DOT
        PoolableParticle activeParticle =
            ObjectPooler.Instance.GetPooledObject(dot.particleEffect.GetComponent<IPoolable>().PoolKey)
                .GetComponent<PoolableParticle>();

        // Take away health while the dot still has ticks and is active
        while (ticks > 0 && dot.isActive)
        {
            // TODO: THIS ASSUMES WE ALWAYS DO DAMAGE!
            _healthScript.ModifyHealth(-1f * dot.damage);

            StartCoroutine(StartParticleEffect(activeParticle, dot));

            yield return new WaitForSeconds(dot.tickInterval);

            ticks--;
        }

        RemoveDamageOverTime(dot);
    }

    ///-///////////////////////////////////////////////////////////
    /// Reapply the expired DOT to this target, if they are still colliding with the
    /// AOE that originally applied it.
    ///
    private void ReapplyDot(DamageOverTime dotExpired)
    {
        AreaOfEffectData sourceOfDot = dotExpired.source;

        // If this target is still standing in an AOE, reapply the DOT
        if (AreaOfEffectManager.Instance.IsTargetOverlappingAreaOfEffect(sourceOfDot, gameObject))
        {
            AreaOfEffectManager.Instance.TryAddAffectedTarget(sourceOfDot, gameObject);

            // Add the DOT to this target again
            AddDamageOverTime(dotExpired);

        }
        else
        {
            // Remove the DOT from the target
            AreaOfEffectManager.Instance.RemoveTargetFromAffectedHashSet(sourceOfDot, gameObject);
            
            dotExpired.isActive = false;
        }


    }

    public IEnumerator StartParticleEffect(PoolableParticle particleEffect, Modifier modifier)
    {
        particleEffect.PlaceParticleOnTransform(transform);

        particleEffect.PlayAllParticles(1f);

        yield break;
    }
}
    
