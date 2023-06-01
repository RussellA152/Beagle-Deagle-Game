[System.Serializable]
public class DamageModifier: Modifier
{
    public float bonusDamage;
    public DamageModifier(string name, float damage)
    {
        modifierName = name;
        bonusDamage = damage;
    }

}

[System.Serializable]
public class PenetrationModifier: Modifier
{
    public int bonusPenetration;
    public PenetrationModifier(string name, int penetration)
    {
        modifierName = name;
        bonusPenetration = penetration;
    }
}

[System.Serializable]
public class SpreadModifier: Modifier
{
    public float bonusSpread;
    public SpreadModifier(string name, float spread)
    {
        modifierName = name;
        bonusSpread = spread;
    }
}
[System.Serializable]
public class ReloadSpeedModifier: Modifier
{
    public float bonusReloadSpeed;
    public ReloadSpeedModifier(string name, float reloadSpeed)
    {
        modifierName = name;
        bonusReloadSpeed = reloadSpeed;
    }
}

[System.Serializable]
public class AttackSpeedModifier: Modifier
{
    public float bonusAttackSpeed;
    public AttackSpeedModifier(string name, float attackSpeed)
    {
        modifierName = name;
        bonusAttackSpeed = attackSpeed;

    }
}

[System.Serializable]
public class AmmoLoadModifier: Modifier
{
    public float bonusAmmoLoad;
    public AmmoLoadModifier(string name, float ammoLoad)
    {
        modifierName = name;
        bonusAmmoLoad = ammoLoad;
    }
}

