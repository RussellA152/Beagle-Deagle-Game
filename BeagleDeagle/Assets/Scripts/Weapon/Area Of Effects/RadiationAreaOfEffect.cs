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

    private DamageOverTime radiationOverTime;

    public override void OnEnable()
    {
        // Clear all subscriptions to targets' events*
        foreach (GameObject targetWithDOT in affectedEnemies)
        {
            //targetWithDOT.GetComponent<AIHealth>().dotEndEvent -= ReapplyDOT;
            targetWithDOT.GetComponent<IDamageOverTimeHandler>().OnDamageOverTimeExpire -= ReapplyDOT;
        }

        base.OnEnable();

        // The radiation DOT that we will apply to the targets
        // Damage is negative so that we can hurt the target
        radiationOverTime = new DamageOverTime(this.name, -1f * radiationDamage, radiationTicks, radiationTickInterval);
    }

    ///-///////////////////////////////////////////////////////////
    /// When the target enters the radiation AOE, apply a DOT to them,
    /// then subscribe to an event that will wait until that DOT ends.
    /// 
    public override void OnAreaEnter(GameObject target)
    {
        base.OnAreaEnter(target);

        IDamageOverTimeHandler damageOverTimeScript = target.GetComponent<IDamageOverTimeHandler>();

        damageOverTimeScript.OnDamageOverTimeExpire += ReapplyDOT;

        Debug.Log("Subscribe to DOT event!");
    }

    ///-///////////////////////////////////////////////////////////
    /// Apply DOT to the target
    ///
    public override void AddEffectOnEnemies(GameObject target)
    {

        IDamageOverTimeHandler damageOverTimeScript = target.GetComponent<IDamageOverTimeHandler>();

        damageOverTimeScript.AddDamageOverTime(radiationOverTime);


        Debug.Log("Radiation was applied!");

    }

    ///-///////////////////////////////////////////////////////////
    /// When the target exits the radiation AOE, unsubscribe from DOT end event.
    /// This prevents the AOE from trying to reapply the DOT when the target leaves the area
    ///
    public override void RemoveEffectFromEnemies(GameObject target)
    {
        IDamageOverTimeHandler damageOverTimeScript = target.GetComponent<IDamageOverTimeHandler>();

        damageOverTimeScript.OnDamageOverTimeExpire -= ReapplyDOT;
    }

    ///-///////////////////////////////////////////////////////////
    /// Reapply DOT to target if the DOT that ended originated from this AOE
    ///
    public void ReapplyDOT(Tuple<GameObject, DamageOverTime> tuple)
    {
        // Check if the DOT that ended was the same as this DOT (radiation)
        if(tuple.Item2 == radiationOverTime)
        {
            Debug.Log("Reapply DOT NOW! TO " + tuple.Item1.name);

            AddEffectOnEnemies(tuple.Item1);
        }
        
    }

}
