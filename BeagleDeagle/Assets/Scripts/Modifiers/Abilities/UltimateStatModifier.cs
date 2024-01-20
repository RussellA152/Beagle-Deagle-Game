using UnityEngine;

[System.Serializable]
public class UltimateCooldownModifier: Modifier
{
    [Range(-1f, 1f)]
    public float bonusUltimateCooldown;

    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to cooldown rate of the player's
    /// utility ability.
    /// 
    public UltimateCooldownModifier(string name, float cooldown)
    {
        modifierName = name;
        bonusUltimateCooldown = cooldown;
    }
}

[System.Serializable]
public class UltimateDamageModifier : Modifier
{
    [Range(-3f, 3f)]
    public float bonusUltimateDamage;

    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to an ultimate ability's damage amount.
    /// 
    public UltimateDamageModifier(string name, float damage)
    {
        modifierName = name;
        bonusUltimateDamage = damage;
    }
}

