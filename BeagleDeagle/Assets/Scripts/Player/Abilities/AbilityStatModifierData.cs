using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityStats", menuName = "ScriptableObjects/Stat Modifiers/All Ability Stats")]
public class AbilityStatModifierData : ScriptableObject, IHasDescription
{
    [Header("Utility Modifiers")]
    public UtilityDamageModifier utilityDamageModifier;
    
    public UtilityUsesModifier utilityUsesModifier;

    public UtilityCooldownModifier utilityCooldownModifier;

    [Header("Ultimate Modifiers")] 
    public UltimateDamageModifier ultimateDamageModifier;
    
    public UltimateCooldownModifier ultimateCooldownModifier;
    

    [SerializeField, Space(10), TextArea(2,3)] 
    private string description;

    public string GetDescription()
    {
        return description;
    }
}
