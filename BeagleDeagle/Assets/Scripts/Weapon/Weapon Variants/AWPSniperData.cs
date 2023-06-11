using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/Weapon/AwpGun")]
public class AWPSniperData : GunData
{
    public override IEnumerator WaitReload(float reloadTimeModifier)
    {
        yield return null;

        Debug.Log("Cannot reload this weapon!");
    }
}
