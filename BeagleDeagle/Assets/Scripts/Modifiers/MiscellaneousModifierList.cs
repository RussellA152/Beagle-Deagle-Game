using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MiscellaneousModifierList : MonoBehaviour, IRegisterModifierMethods
{
    private ModifierManager _modifierManager;

    [Header("Modifiers")] [SerializeField, NonReorderable]
    private List<ExplosiveRadiusModifier> explosiveRadiusModifiers = new List<ExplosiveRadiusModifier>();

    [SerializeField, NonReorderable, Space(10)]
    private List<AreaOfEffectRadiusModifier> aoeRadiusModifiers = new List<AreaOfEffectRadiusModifier>();
    
    [SerializeField, NonReorderable, Space(10)]
    private List<DamageOverTimeDamageModifier> dotDamageModifier = new List<DamageOverTimeDamageModifier>();
    
    public float BonusExplosiveRadius { get; private set; } = 1f;
    public float BonusAoeRadius { get; private set; } = 1f;
    
    public float BonusDotDamage { get; private set; } = 1f;

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

    public void AddAoeRadiusModifier(AreaOfEffectRadiusModifier modifierToAdd)
    {
        if (aoeRadiusModifiers.Contains(modifierToAdd)) return;
        
        aoeRadiusModifiers.Add(modifierToAdd);
        BonusAoeRadius += modifierToAdd.bonusRadius;
    }
    
    public void RemoveAoeRadiusModifier(AreaOfEffectRadiusModifier modifierToRemove)
    {
        if (!aoeRadiusModifiers.Contains(modifierToRemove)) return;
        
        aoeRadiusModifiers.Remove(modifierToRemove);
        BonusAoeRadius /= (1 + modifierToRemove.bonusRadius);
    }
    
    public void AddDotDamageModifier(DamageOverTimeDamageModifier modifierToAdd)
    {
        if (dotDamageModifier.Contains(modifierToAdd)) return;
        
        dotDamageModifier.Add(modifierToAdd);
        BonusDotDamage += modifierToAdd.bonusDamage;
    }
    
    public void RemoveDotDamageModifier(DamageOverTimeDamageModifier modifierToRemove)
    {
        if (!dotDamageModifier.Contains(modifierToRemove)) return;
        
        dotDamageModifier.Remove(modifierToRemove);
        BonusAoeRadius /= (1 + modifierToRemove.bonusDamage);
    }
    

    public void RegisterAllAddModifierMethods()
    {
        _modifierManager.RegisterAddMethod<ExplosiveRadiusModifier>(AddExplosiveRadiusModifier);
        _modifierManager.RegisterAddMethod<AreaOfEffectRadiusModifier>(AddAoeRadiusModifier);
        _modifierManager.RegisterAddMethod<DamageOverTimeDamageModifier>(AddDotDamageModifier);
    }

    public void RegisterAllRemoveModifierMethods()
    {
        _modifierManager.RegisterRemoveMethod<ExplosiveRadiusModifier>(RemoveExplosiveRadiusModifier);
        _modifierManager.RegisterRemoveMethod<AreaOfEffectRadiusModifier>(RemoveAoeRadiusModifier);
        _modifierManager.RegisterRemoveMethod<DamageOverTimeDamageModifier>(RemoveDotDamageModifier);
    }
}
