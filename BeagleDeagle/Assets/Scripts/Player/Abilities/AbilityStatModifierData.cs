using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityStats", menuName = "ScriptableObjects/Stat Modifiers/All Ability Stats")]
public class AbilityStatModifierData : ScriptableObject, IHasDescription
{
    [Header("Utility Modifiers")]
    public UtilityUsesModifier UtilityUsesModifier;

    public UtilityCooldownModifier UtilityCooldownModifier;

    [Header("Ultimate Modifiers")]
    public UltimateCooldownModifier UltimateCooldownModifier;
    

    [SerializeField, Space(10), TextArea(2,3)] 
    private string description;

    public string GetDescription()
    {
        return description;
    }
}
