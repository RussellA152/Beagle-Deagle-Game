using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityWeapon", menuName = "ScriptableObjects/Weapon/AbilityWeapon")]
public class AbilityGunData : GunData
{
    [Header("Ability That Activates This")]
    [SerializeField] private AbilityData abilityData;

    public override float GetDamage()
    {
        // Return the damage of this weapon's associated ability
        // For example, the AWP is an ultimate ability, so return the damage of the ultimate ability
        return abilityData.abilityDamage;
    }

}
