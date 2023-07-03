using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewAreaOfEffect", menuName = "ScriptableObjects/Area of Effects/Nuke Radiation")]
public class DamageAreaOfEffectData : AreaOfEffectData
{
    public DamageOverTime damageOverTime;
    
}
