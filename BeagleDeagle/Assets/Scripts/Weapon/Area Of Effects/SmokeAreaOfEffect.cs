using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAreaOfEffect", menuName = "ScriptableObjects/Area of Effects/Slow Smoke")]
public class SmokeAreaOfEffect : AreaOfEffectData
{
    // A scriptable object for the smoke grenade utility (contains duration)
    public SmokeBombUtility smokeBombData;

    // A positive value representing how much to decrease the enemy's movement speed by (%)
    [Header("Effects On Target")]
    [Range(0f, -1f)]
    [SerializeField]
    private float movementSlowAmount;

    // A positive value representing how much to increase the enemy's attack cooldown by (%)
    [Range(0f, 1f)]
    [SerializeField]
    private float attackSlowAmount;

    private MovementSpeedModifier movementSlowEffect;
    private AttackSpeedModifier attackSlowEffect;


    private void OnEnable()
    {
        // The slow effect to apply to targets
        movementSlowEffect = new MovementSpeedModifier(this.name, movementSlowAmount);
        // The attack speed slow effect to apply to targets
        attackSlowEffect = new AttackSpeedModifier(this.name, attackSlowAmount);
    }

    ///-///////////////////////////////////////////////////////////
    /// When the target enters the smoke AOE, apply the slow and attack slow effects
    /// 
    public override void AddEffectOnEnemies(GameObject target)
    {
        target.GetComponent<IMovable>().AddMovementSpeedModifier(movementSlowEffect);
        target.GetComponent<IDamager>().AddAttackSpeedModifier(attackSlowEffect);
    }

    ///-///////////////////////////////////////////////////////////
    /// When the target exits the smoke AOE, remove the slow and attack slow effects immediately
    /// 
    public override void RemoveEffectFromEnemies(GameObject target)
    {
        target.GetComponent<IMovable>().RemoveMovementSpeedModifier(movementSlowEffect);
        target.GetComponent<IDamager>().RemoveAttackSpeedModifier(attackSlowEffect);
    }

}
