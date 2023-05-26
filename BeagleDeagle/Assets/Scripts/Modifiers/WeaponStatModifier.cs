
[System.Serializable]
public class DamageModifier
{
    public float bonusDamage;
    public DamageModifier(float damage)
    {
        bonusDamage = damage;
    }

}

[System.Serializable]
public class PenetrationModifier
{
    public int bonusPenetration;

    public PenetrationModifier(int penetration)
    {
        bonusPenetration = penetration;
    }
}

[System.Serializable]
public class SpreadModifier
{
    public float bonusSpread;

    public SpreadModifier(float spread)
    {
        bonusSpread = spread;
    }
}
[System.Serializable]
public class ReloadSpeedModifier
{
    public float bonusReloadSpeed;

    public ReloadSpeedModifier(float reloadSpeed)
    {
        bonusReloadSpeed = reloadSpeed;
    }
}

[System.Serializable]
public class FireRateModifier
{
    public float bonusFireRate;

    public FireRateModifier(float fireRate)
    {
        bonusFireRate = fireRate;
    }
}

[System.Serializable]
public class AmmoLoadModifier
{
    public float bonusAmmoLoad;

    public AmmoLoadModifier(float ammoLoad)
    {
        bonusAmmoLoad = ammoLoad;
    }
}

