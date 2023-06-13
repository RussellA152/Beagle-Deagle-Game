using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityWeapon", menuName = "ScriptableObjects/Weapon/AbilityWeapon")]
public class AbilityGunData : GunData
{
    [Header("Ability That Activates This")]
    [SerializeField]
    private AbilityData abilityData;

    public override float GetDamage()
    {
        return abilityData.abilityDamage;
    }

    public override IEnumerator WaitReload(float reloadTimeModifier)
    {
        yield return null;

        Debug.Log("Cannot reload this weapon!");
    }
}
