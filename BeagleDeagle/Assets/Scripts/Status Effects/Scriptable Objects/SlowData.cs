using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSlowEffect", menuName = "ScriptableObjects/StatusEffects/Slow")]
public class SlowData : StatusEffectData
{
    public MovementSpeedModifier movementSpeedEffect;

    public AttackSpeedModifier attackSpeedEffect;
}
