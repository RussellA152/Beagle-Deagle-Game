using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAreaOfEffect", menuName = "ScriptableObjects/Area of Effects/Slow Smoke")]
public class SmokeAreaOfEffect : AreaOfEffectData
{
    public SmokeBombUtility smokeBombData;

    [Header("Effects On Target")]
    [Range(0f, -1f)]
    [SerializeField]
    private float movementSlowAmount; // A positive value representing how much to decrease the enemy's movement speed by (%)

    [Range(0f, 1f)]
    [SerializeField]
    private float attackSlowAmount; // A positive value representing how much to increase the enemy's attack cooldown by (%)


    private MovementSpeedModifier movementSlowEffect;
    private AttackSpeedModifier attackSlowEffect;


    public override void OnEnable()
    {
        base.OnEnable();
        movementSlowEffect = new MovementSpeedModifier(this.name, movementSlowAmount);
        attackSlowEffect = new AttackSpeedModifier(this.name, attackSlowAmount);
    }

    public override void AddEffectOnEnemies(Collider2D targetCollider)
    {
        targetCollider.gameObject.GetComponent<IMovable>().AddMovementSpeedModifier(movementSlowEffect);
        targetCollider.gameObject.GetComponent<IDamager>().AddAttackSpeedModifier(attackSlowEffect);
    }

    public override void RemoveEffectFromEnemies(Collider2D targetCollider)
    {
        targetCollider.gameObject.GetComponent<IMovable>().RemoveMovementSpeedModifier(movementSlowEffect);
        targetCollider.gameObject.GetComponent<IDamager>().RemoveAttackSpeedModifier(attackSlowEffect);
    }

}
