using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeHandler : MonoBehaviour, IDamageOverTimeHandler
{
    // The health script of this target
    private IHealth _healthScript;
    private ModifierParticleEffectHandler _modifierParticleEffectHandler;

    [Header("All Damage Over Time Effects Afflicted With")]
    // All damage over time effects that have been applied to this target
    [SerializeField, NonReorderable]
    private List<DamageOverTime> damageOverTimeEffects = new List<DamageOverTime>();

    private void OnEnable()
    {
        _healthScript ??= GetComponent<IHealth>();
        _modifierParticleEffectHandler = GetComponent<ModifierParticleEffectHandler>();
    }

    public void RevertAllModifiers()
    {
        damageOverTimeEffects.Clear();
    }

    ///-///////////////////////////////////////////////////////////
    /// Add a new DamageOverTime effect to this target,
    /// then start a coroutine that will wait some time to remove it.
    public void AddDamageOverTime(DamageOverTime dotToAdd)
    {
        damageOverTimeEffects.Add(dotToAdd);

        StartCoroutine(TakeDamageOverTime(dotToAdd);

    }

    ///-///////////////////////////////////////////////////////////
    /// Make the target take damage (or heal) every "tickInterval" seconds for a 
    /// "tick" amount of times.
    /// 
    public IEnumerator TakeDamageOverTime(DamageOverTime dot)
    {
        float ticks = dot.ticks;

        // Take away health while the dot still has ticks and is active
        while (ticks > 0)
        {
            _modifierParticleEffectHandler.StartPlayingParticle(dot, dot.StickToGameObject);
            
            // TODO: THIS ASSUMES WE ALWAYS DO DAMAGE!
            _healthScript.ModifyHealth((-1f * dot.damage) );

            yield return new WaitForSeconds(dot.tickInterval);

            ticks--;
        }

        if (CheckIfCanReapply(dot))
        {
            StartCoroutine(TakeDamageOverTime(dot));
        }
        //RemoveDamageOverTime(dot);
    }

    ///-///////////////////////////////////////////////////////////
    /// Reapply the expired DOT to this target, if they are still colliding with the
    /// AOE that originally applied it.
    ///
    private bool CheckIfCanReapply(DamageOverTime dotExpired)
    {
        AreaOfEffectData sourceOfDot = dotExpired.source;

        // If this target is still standing in an AOE, reapply the DOT
        if (AreaOfEffectManager.Instance.IsTargetOverlappingAreaOfEffect(sourceOfDot, gameObject))
        {
            AreaOfEffectManager.Instance.TryAddAffectedTarget(sourceOfDot, gameObject);
            
            return true;

        }
        // Otherwise, don't reapply the DOT and remove it entirely.

        AreaOfEffectManager.Instance.RemoveTargetFromAffectedHashSet(sourceOfDot, gameObject);

        _modifierParticleEffectHandler.StopSpecificParticle(dotExpired);

        damageOverTimeEffects.Remove(dotExpired);

        return false;
    }

}
    
