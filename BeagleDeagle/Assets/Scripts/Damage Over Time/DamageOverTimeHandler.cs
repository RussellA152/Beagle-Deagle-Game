using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeHandler : MonoBehaviour, IDamageOverTimeHandler
{
    // The health script of this entity
    private IHealth _healthScript;

    // All damage over time effects that have been applied to this entity
    [SerializeField, NonReorderable]
    private List<DamageOverTime> damageOverTimeEffects = new List<DamageOverTime>();

    // An event that occurs when any damage over time effect expires
    public event Action<DamageOverTime> onDamageOverTimeExpire;

    private void OnEnable()
    {
        _healthScript ??= GetComponent<IHealth>();
        onDamageOverTimeExpire += ReapplyDOT;
    }

    private void OnDisable()
    {
        onDamageOverTimeExpire -= ReapplyDOT;
    }

    public void RevertAllModifiers()
    {
        damageOverTimeEffects.Clear();
    }

    public void AddDamageOverTime(DamageOverTime dotToAdd)
    {
        damageOverTimeEffects.Add(dotToAdd);
        

        StartCoroutine(TakeDamageOverTime(dotToAdd));
    }

    public void RemoveDamageOverTime(DamageOverTime dotToRemove)
    {
        damageOverTimeEffects.Remove(dotToRemove);

        if (onDamageOverTimeExpire != null)
        {
            //Tuple<GameObject, DamageOverTime> tuple = Tuple.Create(this.gameObject, dotToRemove);
            
            onDamageOverTimeExpire(dotToRemove);
        }
        

    }

    public IEnumerator TakeDamageOverTime(DamageOverTime dot)
    {
        float ticks = dot.ticks;

        while (ticks > 0)
        {
            // THIS ASSUMES WE ALWAYS DO DAMAGE!
            _healthScript.ModifyHealth(dot.damage);

            yield return new WaitForSeconds(dot.tickInterval);

            ticks--;
        }

        RemoveDamageOverTime(dot);
    }

    ///-///////////////////////////////////////////////////////////
    /// Reapply DOT to target if the DOT that ended originated from this AOE
    ///
    private void ReapplyDOT(DamageOverTime dotExpired)
    {
        //AreaOfEffectData sourceOfDOT = tuple.Item2.source;
        AreaOfEffectData sourceOfDot = dotExpired.source;

        // If this target is still standing in an AOE, reapply the DOT
        if (AreaOfEffectManager.Instance.IsTargetOverlappingAreaOfEffect(sourceOfDot, gameObject))
        {
            Debug.Log("TRUE, REAPPLY DOT");
            
            AreaOfEffectManager.Instance.TryAddAffectedTarget(sourceOfDot, gameObject);
            
            sourceOfDot.AddEffectOnEnemies(gameObject);
        }
        else
        {
            Debug.Log("FALSE, DO NOT REAPPLY DOT");
            AreaOfEffectManager.Instance.RemoveTargetFromAffectedHashSet(sourceOfDot, gameObject);
        }


    }
    
}
