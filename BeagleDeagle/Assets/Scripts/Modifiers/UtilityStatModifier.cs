
[System.Serializable]
public class UtilityUsesModifier
{
    public int bonusUtilityUses;

    public UtilityUsesModifier(int uses)
    {
        bonusUtilityUses = uses;
    }
}
[System.Serializable]
public class UtilityCooldownModifier
{
    public float bonusUtilityCooldown;

    public UtilityCooldownModifier(float cooldown)
    {
        bonusUtilityCooldown = cooldown;
    }
}