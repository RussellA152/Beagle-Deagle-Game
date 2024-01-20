using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeOnCollision : StatusEffect<DamageOverTimeData>//, IHasMiscellaneousModifier
{
    //private float _bonusDotDamage = 1f;
    
    public override void ApplyEffect(GameObject objectHit)
    {
        if (DoesThisAffectTarget(objectHit))
        {
            IDamageOverTimeHandler damageOverTimeScript = objectHit.GetComponent<IDamageOverTimeHandler>();

            damageOverTimeScript?.AddDamageOverTime(StatusEffectData.damageOverTime);
        }
    }

    // public void GiveMiscellaneousModifierList(MiscellaneousModifierList miscellaneousModifierList)
    // {
    //     _bonusDotDamage = miscellaneousModifierList.BonusDotDamage;
    // }
    //
    // public void ResetMiscellaneousBonuses()
    // {
    //     _bonusDotDamage = 1f;
    // }
}
