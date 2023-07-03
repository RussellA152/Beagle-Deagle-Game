using UnityEngine;

[System.Serializable]
public class UtilityUsesModifier: Modifier
{
    [Range(-10, 10)]
    public int bonusUtilityUses;
    
    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to the player's amount of utility uses.
    /// 
    public UtilityUsesModifier(string name, int uses)
    {
        modifierName = name;
        bonusUtilityUses = uses;
    }
}

[System.Serializable]
public class UtilityCooldownModifier: Modifier
{
    [Range(-1f, 1f)]
    public float bonusUtilityCooldown;

    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to cooldown rate of the player's
    /// utility ability.
    /// 
    public UtilityCooldownModifier(string name, float cooldown)
    {
        modifierName = name;
        bonusUtilityCooldown = cooldown;
    }
}