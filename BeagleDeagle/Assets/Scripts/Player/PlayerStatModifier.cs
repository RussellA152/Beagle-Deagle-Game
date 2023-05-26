using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatModifier : MonoBehaviour, IPlayerStatModifier
{
    [SerializeField]
    private PlayerHealth playerHealthScript;

    [SerializeField]
    private Abilities abilityScript;

    [Header("Health & Movement Speed Modifiers (%)")]
    [SerializeField]
    private float maxHealthModifier = 1f;// how much extra max health (%) does the player have?
    [SerializeField]
    private float movementSpeedModifier = 1f;// how much extra speed (%) does the player have? *(lower percentages means the player moves slower than usual!)*

    [Header("Weapon Stat Modifiers")]
    [SerializeField]
    private float weaponDamageModifier = 1f; // how much extra damage (%) does the player's weapon do?
    [SerializeField]
    private int weaponPenetrationCountModifier = 0; // how much extra penetration (int) does the player's gun do?
    [SerializeField]
    private float weaponSpreadModifier = 1f;// how much more or less spread (%) does the player's weapon have when it shoots?
    [SerializeField]
    private float weaponMagazineModifier = 1f;// how much more or less bullets (%) can the player's gun shoot?
    [SerializeField]
    private float weaponFireRateModifier = 1f; // how much faster or slower (%) can the player's gun shoot?
    [SerializeField]
    private float weaponReloadSpeedModifier = 1f; // how much faster or slower (%) can the player reload their gun?

    [Header("Utility Ability Modifiers")]
    [SerializeField]
    private float utilityCooldownModifier = 1f;// how much quicker or slower (%) does the player's utility ability go on cooldown? *(higher percentages mean longer cooldowns!)*
    [SerializeField]
    private int utilityUsesModifier = 0; // how many more or less times (int) can the player activate their utility ability?

    //private void Start()
    //{
    //    Invoke("IncreaseRandom", 3f);
    //}

    //private void IncreaseRandom()
    //{
    //    Debug.Log("Increase! DEBUG");
    //    ModifyUtilityUsesModifier(3);
    //}

    // Setter methods for modifiers
    public void ModifyDamageModifier(float amount)
    {
        weaponDamageModifier += amount;
    }

    public void ModifyMagazineSizeModfier(float amount)
    {
        weaponMagazineModifier += amount;
    }

    public void ModifyMaxHealthModifier(float amount)
    {
        maxHealthModifier += amount;
        // call function in health script to invoke event system
        playerHealthScript.MaxHealthWasModified();
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

    public void ModifyUtilityUsesModifier(int amount)
    {
        utilityUsesModifier += amount;
        //abilityScript.UtilityUsesModified();
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
        return maxHealthModifier;
    }

    public float GetDamageModifier()
    {
        return weaponDamageModifier;
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
        return weaponMagazineModifier;
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
