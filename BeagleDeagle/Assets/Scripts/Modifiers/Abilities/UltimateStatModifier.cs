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

