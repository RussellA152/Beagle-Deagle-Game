using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsModifier : MonoBehaviour, IPlayerStatModifier
{
    [Header("Health & Movement Speed Modifers")]
    [SerializeField]
    private float maxHealthModifer; // how much extra max health (%) does the player have?

    [SerializeField]
    private float movementSpeedModifier; // how much extra speed (%) does the player do? (lower percentages means the player moves slower than usual)

    [Header("Utility Ability Modifiers")]
    [SerializeField]
    private float utilityCooldownModifier; // how much quicker or slower (%) does the player's utility ability go on cooldown?

    [SerializeField]
    private int utilityUsesModifier; // how many more or less times (int) can the player activate their utility ability?

    [Header("Weapon Modifers")]
    [SerializeField]
    private float weaponDamageModifer; // how much extra damage (%) does the player's weapon do?

    [SerializeField]
    private int weaponPenetrationCountModifier; // how much extra penetration (int) does the player's gun do?

    [SerializeField]
    private float weaponSpreadModifier; // how much more or less spread (%) does the player's weapon have when it shoots?

    [SerializeField]
    private float weaponMagazineModifer; // how much more or less bullets (%) can the player's gun shoot?

    [SerializeField]
    private float weaponReloadSpeedModifier; // how much faster or slower (%) can the player reload their gun?


    public void ModifyDamageModifer(float amount)
    {
        weaponDamageModifer += amount;
    }

    public void ModifyMagazineSizeModfier(float amount)
    {
        weaponMagazineModifer += amount;
    }

    public void ModifyMaxHealthModifier(float amount)
    {
        maxHealthModifer += amount;
    }

    public void ModifyPenetrationCountModifier(int amount)
    {
        // could pose an issue if the penetration count goes lower than 1...
        weaponPenetrationCountModifier += amount;
    }

    public void ModifySpeedModifer(float amount)
    {
        movementSpeedModifier += amount;
    }

    public void ModifyUtilityCooldownModifier(float amount)
    {
        utilityCooldownModifier += amount;
    }

    public void ModifyUtilityUsesModifer(int amount)
    {
        utilityUsesModifier += amount;
    }

    public void ModifyWeaponReloadSpeedModifier(float amount)
    {
        weaponReloadSpeedModifier += amount;
    }

    public void ModifyWeaponSpreadModifier(float amount)
    {
        weaponSpreadModifier += amount;
    }
}
