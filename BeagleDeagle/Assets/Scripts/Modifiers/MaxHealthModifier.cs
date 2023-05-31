[System.Serializable]
public class MaxHealthModifier: Modifier
{
    public float bonusMaxHealth;
    public MaxHealthModifier(string name, float maxHealth)
    {
        modifierName = name;
        bonusMaxHealth = maxHealth;
    }
}
