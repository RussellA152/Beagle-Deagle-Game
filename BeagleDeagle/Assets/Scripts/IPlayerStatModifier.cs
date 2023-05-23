using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerStatModifier : IModifier
{
    public void ModifyWeaponSpreadModifier(float amount);

    public void ModifyWeaponReloadSpeedModifier(float amount);

    public void ModifyMagazineSizeModfier(float amount);

    public void ModifyPenetrationCountModifier(int amount);

    public void ModifyUtilityCooldownModifier(float amount);

    public void ModifyUtilityUsesModifer(int amount);

}
