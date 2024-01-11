using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "ScriptableObjects/Stat Modifiers/Weapon")]
public class WeaponStatModifierData : ScriptableObject, IHasDescription 
{
    public DamageModifier DamageModifier;

    public PenetrationModifier PenetrationModifier;

    public SpreadModifier SpreadModifier;

    public ReloadSpeedModifier ReloadSpeedModifier;

    public AttackSpeedModifier AttackSpeedModifier;

    public AmmoLoadModifier AmmoLoadModifier;

    public CriticalChanceModifier CriticalChanceModifier;

    [SerializeField, Space(10), TextArea(2,3)] 
    private string description;
    
    public string GetDescription()
    {
        return description;
    }
}
