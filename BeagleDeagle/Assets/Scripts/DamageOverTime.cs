[System.Serializable]
public class DamageOverTime
{
    public string dotName;

    public float damage;

    //public float duration;

    public float ticks;

    public float tickInterval;

    public DamageOverTime(string name, float damage, float ticks = 0f, float tickInterval = 0f)
    {
        dotName = name;

        this.damage = damage;

        //this.duration = duration;

        this.ticks = ticks;

        this.tickInterval = tickInterval;

    }

}
