using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAreaOfEffect", menuName = "ScriptableObjects/Area of Effects/Nuke Radiation")]
public class RadiationAreaOfEffect : AreaOfEffectData
{
    [Range(0f,100f)]
    public float radiationDamage; // How much damage does this do to enemies per tick?

    [Range(1, 20)]
    public int radiationTicks; // How many times does the radiation hurt the enemy?

    [Range(0f, 30f)]
    public float radiationTickInterval; // How much time between each tick?

    private DamageOverTime radiationOverTime;

    public override void OnEnable()
    {
        base.OnEnable();
        radiationOverTime = new DamageOverTime(this.name, radiationDamage, radiationTicks, radiationTickInterval);
    }

    public override void AddEffectOnEnemies(Collider2D targetCollider)
    {
        targetCollider.gameObject.GetComponent<IHealth>().AddDamageOverTime(radiationOverTime);
    }

    public override void RemoveEffectFromEnemies(Collider2D targetCollider)
    {
        throw new System.NotImplementedException();
    }

}
