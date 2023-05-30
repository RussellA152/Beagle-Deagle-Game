
[System.Serializable]
public class DamageModifier
{
    public string modifierName;
    public float bonusDamage;
    public bool appliedOnTriggerEnter;

    public DamageModifier(string name, float damage)
    {
        bonusDamage = damage;
    }

}

[System.Serializable]
public class PenetrationModifier
{
    public string modifierName;
    public int bonusPenetration;


    public PenetrationModifier(string name, int penetration)
    {
        modifierName = name;
        bonusPenetration = penetration;
    }
}

[System.Serializable]
public class SpreadModifier
{
    public string modifierName;
    public float bonusSpread;

    public SpreadModifier(string name, float spread)
    {
        modifierName = name;
        bonusSpread = spread;
    }
}
[System.Serializable]
public class ReloadSpeedModifier
{
    public string modifierName;
    public float bonusReloadSpeed;

    public ReloadSpeedModifier(string name, float reloadSpeed)
    {
        modifierName = name;
        bonusReloadSpeed = reloadSpeed;
    }
}

[System.Serializable]
public class AttackSpeedModifier
{
    public string modifierName;
    public float bonusAttackSpeed;
    public bool appliedOnTriggerEnter;

    public AttackSpeedModifier(string name, float attackSpeed, bool isAppliedOnTriggerEnter)
    {
        modifierName = name;
        bonusAttackSpeed = attackSpeed;
        appliedOnTriggerEnter = isAppliedOnTriggerEnter;


    }
}

[System.Serializable]
public class AmmoLoadModifier
{
    public string modifierName;
    public float bonusAmmoLoad;

    public AmmoLoadModifier(string name, float ammoLoad)
    {
        modifierName = name;
        bonusAmmoLoad = ammoLoad;
    }
}

