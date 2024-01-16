using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MiscellaneousModifierList : MonoBehaviour, IRegisterModifierMethods
{
    private ModifierManager _modifierManager;

    [Header("Modifiers")]

    [SerializeField, NonReorderable]
    private List<ExplosiveRadiusModifier> explosiveRadiusModifiers = new List<ExplosiveRadiusModifier>(); // display all modifiers applied to the explosives (for debugging mainly)
    
    public float BonusExplosiveRadius { get; private set; } = 1f;

    private void Awake()
    {
        _modifierManager = GetComponent<ModifierManager>();
        
        RegisterAllAddModifierMethods();
        RegisterAllRemoveModifierMethods();
    }

    private void OnDisable()
    {
        explosiveRadiusModifiers.Clear();
    }

    public void AddExplosiveRadiusModifier(ExplosiveRadiusModifier modifierToAdd)
    {
        if (explosiveRadiusModifiers.Contains(modifierToAdd)) return;
        
        explosiveRadiusModifiers.Add(modifierToAdd);
        BonusExplosiveRadius += modifierToAdd.bonusRadius;
        
    }

    public void RemoveExplosiveRadiusModifier(ExplosiveRadiusModifier modifierToRemove)
    {
        if (!explosiveRadiusModifiers.Contains(modifierToRemove)) return;
        
        explosiveRadiusModifiers.Remove(modifierToRemove);
        BonusExplosiveRadius /= (1 + modifierToRemove.bonusRadius);
        
    }

    public void RegisterAllAddModifierMethods()
    {
        _modifierManager.RegisterAddMethod<ExplosiveRadiusModifier>(AddExplosiveRadiusModifier);
    }

    public void RegisterAllRemoveModifierMethods()
    {
        _modifierManager.RegisterRemoveMethod<ExplosiveRadiusModifier>(RemoveExplosiveRadiusModifier);
    }
}
