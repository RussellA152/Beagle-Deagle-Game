using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusEffect : IDataUpdatable<StatusEffectData>
{
    public void UpdateWeaponType(WeaponType weaponTypeScriptableObject);
}
