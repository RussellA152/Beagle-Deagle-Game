using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSlowEffect", menuName = "ScriptableObjects/StatusEffects/Slow")]
public class SlowData : StatusEffectData
{
    // The effect that the slow will apply to movement speed
    public MovementSpeedModifier movementSpeedEffect;

    // The effect that the slow will apply to attack speed
    public AttackSpeedModifier attackSpeedEffect;

    [Range(0.1f, 30f)]
    // How long should attack and movement speed effect last after its supposed to get removed?
    // Ex. After enemy walks out of smoke cloud, how long does it take to remove the effect from them?
    public float lingerDuration;
}
