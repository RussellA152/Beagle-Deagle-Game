using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IApplyDamageOverTime
{
    ///-///////////////////////////////////////////////////////////
    /// Give bonus damage to the DOT being applied. This doesn't include the 
    /// DamageOverTimeDamage modifier. This is meant for abilities or bullets that apply dots, and can increase dot damage.
    /// 
    public void GiveBonusDamage(float bonusDamage);
}
