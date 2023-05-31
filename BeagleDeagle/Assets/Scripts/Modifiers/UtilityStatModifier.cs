[System.Serializable]
public class UtilityUsesModifier: Modifier
{
    public int bonusUtilityUses;
    public UtilityUsesModifier(string name, int uses)
    {
        modifierName = name;
        bonusUtilityUses = uses;
    }
}

[System.Serializable]
public class UtilityCooldownModifier: Modifier
{
    public float bonusUtilityCooldown;
    public UtilityCooldownModifier(string name, float cooldown)
    {
        modifierName = name;
        bonusUtilityCooldown = cooldown;
    }
}