using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAreaOfEffect", menuName = "ScriptableObjects/Area of Effects/Nuke Radiation")]
public class RadiationAreaOfEffect : AreaOfEffectData
{
    // How much damage does this do to enemies per tick?
    [Range(0f,100f)]
    public float radiationDamage;

    // How many times does the radiation hurt the enemy?
    [Range(1, 20)]
    public int radiationTicks;

    // How much time between each tick?
    [Range(0f, 30f)]
    public float radiationTickInterval;

    private DamageOverTime _radiationOverTime;

    private void OnEnable()
    {
        _radiationOverTime = new DamageOverTime(this.name, -1f * radiationDamage, radiationTicks, radiationTickInterval);
    }

    ///-///////////////////////////////////////////////////////////
    /// When the target enters the radiation AOE, apply a DOT to them,
    /// then subscribe to an event that will wait until that DOT ends.
    /// 
    public override void OnAreaEnter(GameObject target)
    {
        IDamageOverTimeHandler damageOverTimeScript = target.GetComponent<IDamageOverTimeHandler>();

        damageOverTimeScript.onDamageOverTimeExpire += ReapplyDOT;

        Debug.Log("Subscribe to DOT event!");
    }

    ///-///////////////////////////////////////////////////////////
    /// Apply DOT to the target
    ///
    protected override void AddEffectOnEnemies(GameObject target)
    {
        IDamageOverTimeHandler damageOverTimeScript = target.GetComponent<IDamageOverTimeHandler>();

        damageOverTimeScript.AddDamageOverTime(_radiationOverTime);

        Debug.Log("Radiation was applied!");

    }

    ///-///////////////////////////////////////////////////////////
    /// When the target exits the radiation AOE, unsubscribe from DOT end event.
    /// This prevents the AOE from trying to reapply the DOT when the target leaves the area
    ///
    protected override void RemoveEffectFromEnemies(GameObject target)
    {
        IDamageOverTimeHandler damageOverTimeScript = target.GetComponent<IDamageOverTimeHandler>();

        damageOverTimeScript.onDamageOverTimeExpire -= ReapplyDOT;
    }

    ///-///////////////////////////////////////////////////////////
    /// Reapply DOT to target if the DOT that ended originated from this AOE
    ///
    private void ReapplyDOT(Tuple<GameObject, DamageOverTime> tuple)
    {
        // Check if the DOT that ended was the same as this DOT (radiation)
        if(tuple.Item2 == _radiationOverTime)
        {
            Debug.Log("Reapply DOT NOW! TO " + tuple.Item1.name);

            AddEffectOnEnemies(tuple.Item1);
        }
        
    }

}
