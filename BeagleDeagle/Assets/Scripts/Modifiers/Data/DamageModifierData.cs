using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageModifier", menuName = "ScriptableObjects/Stat Modifiers/Damage Modifier")]
public class DamageModifierData : ScriptableObject
{
    public DamageModifier damageModifier;
}
