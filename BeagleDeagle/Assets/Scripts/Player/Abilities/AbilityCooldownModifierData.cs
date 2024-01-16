using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityCooldown", menuName = "ScriptableObjects/Stat Modifiers/Ability Cooldown")]
public class AbilityCooldownModifierData : ScriptableObject
{
    [Header("Utility Modifiers")]
    public UtilityCooldownModifier utilityCooldownModifier;

    [Header("Ultimate Modifiers")]
    public UltimateCooldownModifier ultimateCooldownModifier;
    
}
