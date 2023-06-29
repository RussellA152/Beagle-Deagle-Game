using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/Weapon/Gun")]
public class RegularGunData : GunData
{
    [Header("Damage")]
    [Range(0, 1000f)]
    [SerializeField]
    private float damagePerHit;

    [Header("Reloading")]
    [SerializeField]
    [Range(0f, 30f)]
    private float totalReloadTime;

    public override float GetDamage()
    {
        return damagePerHit;
    }

    public override IEnumerator WaitReload(float reloadTimeModifier)
    {

        yield return new WaitForSeconds(totalReloadTime * reloadTimeModifier);
        
    }
}
