using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStunEffect", menuName = "ScriptableObjects/StatusEffects/Stun")]
public class StunData : StatusEffectData
{
    [Range(0.1f, 15f)]
    public float stunDuration;
}
