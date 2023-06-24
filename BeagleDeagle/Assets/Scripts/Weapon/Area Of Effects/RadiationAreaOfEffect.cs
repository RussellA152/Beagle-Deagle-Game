using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAreaOfEffect", menuName = "ScriptableObjects/Area of Effects/Nuke Radiation")]
public class RadiationAreaOfEffect : AreaOfEffectData
{
    public float radiationDamage; // How much damage does this do to enemies per tick?

    private DamageOverTime radiationOverTime;

    public override void OnEnable()
    {
        base.OnEnable();
        radiationOverTime = new DamageOverTime(this.name, radiationDamage, 5f, 1f);
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
