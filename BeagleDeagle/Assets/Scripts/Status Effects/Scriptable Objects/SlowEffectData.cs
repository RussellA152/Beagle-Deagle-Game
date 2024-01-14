using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewSlowEffect", menuName = "ScriptableObjects/StatusEffects/Slow")]
public class SlowEffectData : StatusEffectData
{
    public enum LingerDurationType
    {
        OnEnter,
        
        OnExit
    }
    
    // The effect that the slow will apply to movement speed
    public MovementSpeedModifier movementSpeedEffect;

    // The effect that the slow will apply to attack speed
    public AttackSpeedModifier attackSpeedEffect;

    [Range(0.1f, 60f)]
    public float lingerDuration;

    public LingerDurationType lingerDurationType;
    

}
