[System.Serializable]
public class DamageOverTime
{
    public string dotName;

    public float damage; // How much damage does this DOT do?

    //public float duration;

    public int ticks; // How many times does this DOT apply its damage?

    public float tickInterval; // How much time between each DOT tick?

    public DamageOverTime(string name, float damage, int ticks, float tickInterval)
    {
        dotName = name;

        this.damage = damage;

        //this.duration = duration;

        this.ticks = ticks;

        this.tickInterval = tickInterval;

    }

}
