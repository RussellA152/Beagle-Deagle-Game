using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MiscellaneousModifierList : MonoBehaviour
{
    [Header("Modifiers")]
    [SerializeField, NonReorderable]
    private List<ExplosiveRadiusModifier> explosiveRadiusModifiers = new List<ExplosiveRadiusModifier>(); // display all modifiers applied to the bonusMaxHealth (for debugging mainly)

    public float BonusExplosiveRadius { get; private set; } = 1f;

    private void OnDisable()
    {
        explosiveRadiusModifiers.Clear();
    }

    public void AddExplosiveRadiusModifier(ExplosiveRadiusModifier modifierToAdd)
    {
        explosiveRadiusModifiers.Add(modifierToAdd);
        BonusExplosiveRadius += modifierToAdd.bonusRadius;
        
    }

    public void RemoveExplosiveRadiusModifier(ExplosiveRadiusModifier modifierToRemove)
    {
        explosiveRadiusModifiers.Remove(modifierToRemove);
        BonusExplosiveRadius /= (1 + modifierToRemove.bonusRadius);
        
    }
}
