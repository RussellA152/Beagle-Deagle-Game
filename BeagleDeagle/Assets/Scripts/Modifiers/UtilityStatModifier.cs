[System.Serializable]
public class UtilityUsesModifier
{
    public string modifierName;
    public int bonusUtilityUses;

    public UtilityUsesModifier(string name, int uses)
    {
        modifierName = name;
        bonusUtilityUses = uses;
    }
}

[System.Serializable]
public class UtilityCooldownModifier
{
    public string modifierName;
    public float bonusUtilityCooldown;

    public UtilityCooldownModifier(string name, float cooldown)
    {
        modifierName = name;
        bonusUtilityCooldown = cooldown;
    }
}