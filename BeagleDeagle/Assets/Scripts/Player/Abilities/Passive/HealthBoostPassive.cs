using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthBoost", menuName = "ScriptableObjects/Ability/Passive/HealthBoost")]
public class HealthBoostPassive : ScriptableObject
{
    public MaxHealthModifier maxHealthModifier;

}

