using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerStatModifier : IModifier
{
    // Setter methods

    /// <summary>
    /// Higher values will make a gun's spread higher and might make it miss more. Lower values will make spread tighter.
    /// </summary>
    /// <param name="amount"></param>
    public void ModifyWeaponSpreadModifier(float amount);

    /// <summary>
    /// Higher values will make a gun take longer to reload. Lower values will make a reload faster.
    /// </summary>
    /// <param name="amount"></param>
    public void ModifyWeaponReloadSpeedModifier(float amount);

    /// <summary>
    /// Higher values will make a gun able to shoot more bullets. Lower values will make a gun able to shoot less.
    /// </summary>
    /// <param name="amount"></param>
    public void ModifyMagazineSizeModfier(float amount);

    /// <summary>
    /// Higher values means a gun will penetrate through more entities. Lower values will make a gun penetrate through less
    /// (*Should not go below 1*)
    /// </summary>
    /// <param name="amount"></param>
    public void ModifyPenetrationCountModifier(int amount);

    /// <summary>
    /// Higher values means a player's utility ability will recharge. Lower values means a player will have to wait longer
    /// for their utility to recharge.
    /// </summary>
    /// <param name="amount"></param>
    public void ModifyUtilityCooldownModifier(float amount);

    /// <summary>
    /// Higher values means a player can use their utility ability more. Lower values means a player can use their
    /// utility ability less times.
    /// </summary>
    /// <param name="amount"></param>
    public void ModifyUtilityUsesModifer(int amount);

    // Getter methods (will be converted from a percentage to a float)
    public float GetWeaponSpreadModifier();

    public float GetWeaponReloadSpeedModifier();

    public float GetMagazineSizeModifier();

    public int GetPenetrationCountModifier();

    public float GetUtilityCooldownModifier();

    public int GetUtilityUsesModifier();


}
