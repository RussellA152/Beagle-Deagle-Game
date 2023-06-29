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
        _radiationOverTime = new DamageOverTime(this.name, -1f * radiationDamage, radiationTicks, radiationTickInterval, this);
    }
    
    ///-///////////////////////////////////////////////////////////
    /// Apply DOT to the target
    ///
    public override void AddEffectOnEnemies(GameObject target)
    {
        IDamageOverTimeHandler damageOverTimeScript = target.GetComponent<IDamageOverTimeHandler>();

        damageOverTimeScript.AddDamageOverTime(_radiationOverTime);

        Debug.Log("Radiation was applied!");

    }

    ///-///////////////////////////////////////////////////////////
    /// When the target exits the radiation AOE, unsubscribe from DOT end event.
    /// This prevents the AOE from trying to reapply the DOT when the target leaves the area
    ///
    public override void RemoveEffectFromEnemies(GameObject target)
    {
        // TODO: Not sure what to put here for Radiation
        // Because it doesn't remove any effects OnTriggerExit
    }


}
