
[System.Serializable]
public class MaxHealthModifier
{
    public string modifierName;
    public float bonusMaxHealth;
    public MaxHealthModifier(string name, float maxHealth)
    {
        modifierName = name;
        bonusMaxHealth = maxHealth;
    }
}
