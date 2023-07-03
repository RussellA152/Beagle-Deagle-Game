using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowAreaOfEffect : AreaOfEffect<SlowAreaOfEffectData>
{
    public override void AddEffectOnEnemies(GameObject target)
    {
        target.GetComponent<IMovable>().AddMovementSpeedModifier(areaOfEffectData.movementSlowEffect);
        
        target.GetComponent<IDamager>().AddAttackSpeedModifier(areaOfEffectData.attackSlowEffect);
    }

    public override void RemoveEffectFromEnemies(GameObject target)
    {
        target.GetComponent<IMovable>().RemoveMovementSpeedModifier(areaOfEffectData.movementSlowEffect);
        
        target.GetComponent<IDamager>().RemoveAttackSpeedModifier(areaOfEffectData.attackSlowEffect);
        
    }
}
