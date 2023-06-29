[System.Serializable]
public class DamageOverTime
{
    public string dotName;

    // How much damage does this DOT do?
    public float damage;
    
    // How many times does this DOT apply its damage?
    public int ticks; 

    // How much time between each DOT tick?
    public float tickInterval;

    // What created this DOT?
    public AreaOfEffectData source;

    ///-///////////////////////////////////////////////////////////
    /// Every "tickInterval" seconds, apply a certain amount of damage to the entity
    /// for a "tick" amount of times.
    public DamageOverTime(string name, float damage, int ticks, float tickInterval, AreaOfEffectData source)
    {
        dotName = name;

        this.damage = damage;

        this.ticks = ticks;

        this.tickInterval = tickInterval;

        this.source = source;

    }

}
