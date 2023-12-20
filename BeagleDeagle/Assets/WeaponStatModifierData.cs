using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityWeapon", menuName = "ScriptableObjects/Weapon/Weapon Modifier")]
public class WeaponStatModifierData : ScriptableObject, IHasDescription 
{
    public DamageModifier DamageModifier;

    public PenetrationModifier PenetrationModifier;

    public SpreadModifier SpreadModifier;

    public ReloadSpeedModifier ReloadSpeedModifier;

    public AttackSpeedModifier AttackSpeedModifier;

    public AmmoLoadModifier AmmoLoadModifier;

    [SerializeField, Space(10), TextArea(2,3)] 
    private string description;
    
    public string GetDescription()
    {
        return description;
    }
}
