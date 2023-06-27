using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeHandler : MonoBehaviour, IDamageOverTimeHandler
{
    // The health script of this entity
    private IHealth healthScript;

    // All damage over time effects that have been applied to this entity
    [SerializeField, NonReorderable]
    private List<DamageOverTime> damageOverTimeEffects = new List<DamageOverTime>();

    // An event that occurs when any damage over time effect expires
    public event Action<Tuple<GameObject, DamageOverTime>> OnDamageOverTimeExpire;

    private void OnEnable()
    {
        if(healthScript == null)
        {
            healthScript = GetComponent<IHealth>();
        }

        //OnDamageOverTimeExpire += DebugDOT;
    }

    private void OnDisable()
    {
        //OnDamageOverTimeExpire -= DebugDOT;
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

        if (OnDamageOverTimeExpire != null)
        {
            Tuple<GameObject, DamageOverTime> tuple = Tuple.Create(this.gameObject, dotToRemove);
            OnDamageOverTimeExpire(tuple);
        }

    }

    public IEnumerator TakeDamageOverTime(DamageOverTime dot)
    {
        float ticks = dot.ticks;

        while (ticks > 0)
        {
            // THIS ASSUMES WE ALWAYS DO DAMAGE! WILL CHANGE!
            healthScript.ModifyHealth(dot.damage);

            yield return new WaitForSeconds(dot.tickInterval);

            ticks--;
        }

        RemoveDamageOverTime(dot);
    }

    //public void DebugDOT(Tuple<GameObject, DamageOverTime> tuple)
    //{
    //    Debug.Log("DOT WAS REMOVED!");
    //}
}
