using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageOverTimeHandler
{
    // An event that occurs when any DOT effect expires on the entity
    event Action<Tuple<GameObject, DamageOverTime>> OnDamageOverTimeExpire;

    ///-///////////////////////////////////////////////////////////
    /// Apply a damage over time effect to the entity.
    /// This starts the "TakeDamageOverTime" coroutine
    ///
    public void AddDamageOverTime(DamageOverTime dotToAdd);

    ///-///////////////////////////////////////////////////////////
    /// Remove a damage over time effect from the entity.
    /// This will invoke the "OnDamageOverTimeExpire" event
    ///
    public void RemoveDamageOverTime(DamageOverTime dotToRemove);

    ///-///////////////////////////////////////////////////////////
    /// A coroutine that counts down every tickInterval seconds and hurts/heals the entity
    ///
    public IEnumerator TakeDamageOverTime(DamageOverTime dot);

}
