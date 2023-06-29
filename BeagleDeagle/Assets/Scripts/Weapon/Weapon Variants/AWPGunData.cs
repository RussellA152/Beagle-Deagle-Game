using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityWeapon", menuName = "ScriptableObjects/Weapon/AWP")]
public class AWPGunData : GunData
{
    [Header("Ultimate Ability That Activates This")]
    [SerializeField]
    private UltimateAbilityData ultimateAbilityData;

    public override float GetDamage()
    {
        return ultimateAbilityData.abilityDamage;
    }

    public override IEnumerator WaitReload(float reloadTimeModifier)
    {
        yield break;

        //Debug.Log("Cannot reload this weapon!");
    }

    // public override void RefillAmmo(out int newBulletsLoaded, out int newBulletsShot)
    // {
    //     newBulletsLoaded = 0;
    //     newBulletsShot = 0;
    // }
}
