using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatModifier : MonoBehaviour, IPlayerStatModifier
{
    [Header("Health & Movement Speed Modifiers (%)")]
    [SerializeField]
    private float maxHealthModifer = 1f;// how much extra max health (%) does the player have?
    [SerializeField]
    private float movementSpeedModifier = 1f;// how much extra speed (%) does the player have? *(lower percentages means the player moves slower than usual!)*

    [Header("Weapon Stat Modifiers")]
    [SerializeField]
    private float weaponDamageModifer = 1f; // how much extra damage (%) does the player's weapon do?
    [SerializeField]
    private int weaponPenetrationCountModifier = 0; // how much extra penetration (int) does the player's gun do?
    [SerializeField]
    private float weaponSpreadModifier = 1f;// how much more or less spread (%) does the player's weapon have when it shoots?
    [SerializeField]
    private float weaponMagazineModifer = 1f;// how much more or less bullets (%) can the player's gun shoot?
    [SerializeField]
    private float weaponFireRateModifier = 1f; // how much faster or slower (%) can the player's gun shoot?
    [SerializeField]
    private float weaponReloadSpeedModifier = 1f; // how much faster or slower (%) can the player reload their gun?

    [Header("Utility Ability Modifiers")]
    [SerializeField]
    private float utilityCooldownModifier = 1f;// how much quicker or slower (%) does the player's utility ability go on cooldown? *(higher percentages mean longer cooldowns!)*
    [SerializeField]
    private int utilityUsesModifier = 0; // how many more or less times (int) can the player activate their utility ability?

    // Setter methods for modifiers
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

    public void ModifyMovementSpeedModifier(float amount)
    {
        movementSpeedModifier += amount;
    }

    public void ModifyAttackSpeedModifier(float amount)
    {
        weaponFireRateModifier += amount;
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

    public float GetMovementSpeedModifier()
    {
        // ex. Return a 50% increase on the player's current movement speed
        return movementSpeedModifier;
    }

    public float GetMaxHealthModifier()
    {
        return maxHealthModifer;
    }

    public float GetDamageModifier()
    {
        return weaponDamageModifer;
    }
    public float GetAttackSpeedModifier()
    {
        return weaponFireRateModifier;
    }

    public float GetWeaponSpreadModifier()
    {
        return weaponSpreadModifier;
    }

    public float GetWeaponReloadSpeedModifier()
    {
        return weaponReloadSpeedModifier;
    }

    public float GetMagazineSizeModifier()
    {
        return weaponMagazineModifer;
    }

    public int GetPenetrationCountModifier()
    {
        return weaponPenetrationCountModifier;
    }

    public float GetUtilityCooldownModifier()
    {
        return utilityCooldownModifier;
    }

    public int GetUtilityUsesModifier()
    {
        return utilityUsesModifier;
    }

}
