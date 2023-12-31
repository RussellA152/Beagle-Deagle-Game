using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSlowEffect", menuName = "ScriptableObjects/StatusEffects/Slow")]
public class SlowEffectData : StatusEffectData
{
    // The effect that the slow will apply to movement speed
    public MovementSpeedModifier movementSpeedEffect;

    // The effect that the slow will apply to attack speed
    public AttackSpeedModifier attackSpeedEffect;
    
}
