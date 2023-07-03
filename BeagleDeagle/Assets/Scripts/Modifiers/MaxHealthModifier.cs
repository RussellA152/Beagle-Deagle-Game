using UnityEngine;

[System.Serializable]

public class MaxHealthModifier: Modifier
{
    [Range(-1f, 1f)]
    public float bonusMaxHealth;
    
    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to a entity's max health.
    /// 
    public MaxHealthModifier(string name, float maxHealth)
    {
        modifierName = name;
        bonusMaxHealth = maxHealth;
    }
}
