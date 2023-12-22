using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnCollision : StatusEffect<HealData>
{
    public override void ApplyEffect(GameObject objectHit)
    {
        // Heal the object when they collide with this object
        objectHit.GetComponent<IHealth>().ModifyHealth(StatusEffectData.healAmount);
    }
}
