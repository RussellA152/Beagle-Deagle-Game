using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthBoost", menuName = "ScriptableObjects/Stat Modifiers/Health Boost")]
public class HealthBoostData : ScriptableObject
{
    // What is the health boost modification applied to the player?
    public MaxHealthModifier maxHealthModifier;

}

