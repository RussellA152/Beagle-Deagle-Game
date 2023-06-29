[System.Serializable]
public class DamageModifier: Modifier
{
    public float bonusDamage;

    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to an entity's damage values.
    /// For player, this is modifier changes the gun's damage values.
    /// 
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

    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to the player's gun's penetration count.
    ///
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

    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to the player's gun's spread.
    /// 
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

    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to the player's gun's reload speed.
    /// 
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

    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease to an entity's attack speed.
    /// For player, this is applied to their gun's fire rate.
    /// 
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

    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease to the player's gun's magazine size.
    /// 
    public AmmoLoadModifier(string name, float ammoLoad)
    {
        modifierName = name;
        bonusAmmoLoad = ammoLoad;
    }
}

