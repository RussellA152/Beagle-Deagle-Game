using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageOverTimeHandler
{
    ///-///////////////////////////////////////////////////////////
    /// Add a new DamageOverTime effect to this target,
    /// then start a coroutine that will wait some time to remove it.
    public void AddDamageOverTime(DamageOverTime dotToAdd, float bonusDamage);
    
}
