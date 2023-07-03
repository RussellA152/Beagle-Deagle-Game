using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewAreaOfEffect", menuName = "ScriptableObjects/Area of Effects/Slow Smoke")]
public class SlowAreaOfEffectData : AreaOfEffectData
{
    public MovementSpeedModifier movementSlowEffect;
    
    public AttackSpeedModifier attackSlowEffect;
    
}
