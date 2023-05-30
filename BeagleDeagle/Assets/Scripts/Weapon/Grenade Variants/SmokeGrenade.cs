using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGrenade", menuName = "ScriptableObjects/Grenade/SmokeGrenade")]
public class SmokeGrenade : GrenadeData
{
    [Range(0f,-1f)]
    [SerializeField]
    private float movementSlowAmount; // how much to slow (%) an enemy's movement speed by?

    [Range(0f, 1f)]
    [SerializeField]
    private float attackSlowAmount; // how much to slow (%) an enemy's attack cooldown by? (make cooldown take longer)

    [SerializeField]
    private LayerMask layersToHit;

    private MovementSpeedModifier movementSlowEffect;

    private AttackSpeedModifier attackSlowEffect;

    private void OnEnable()
    {
        // Cannot stack the same slow effects on the same enemy more than once
        movementSlowEffect = new MovementSpeedModifier(this.name, movementSlowAmount, true);
        attackSlowEffect = new AttackSpeedModifier(this.name, attackSlowAmount, true);
    }

    public override void Explode()
    {
        Debug.Log("Explode smoke grenade!");
    }

    public override void OnAreaEnter(Collider2D collision)
    {
        collision.gameObject.GetComponent<IMovable>().AddMovementSpeedModifier(movementSlowEffect);

        collision.gameObject.GetComponent<IDamager>().AddAttackSpeedModifier(attackSlowEffect);
       //Debug.Log("Slow enemy!");
    }

    public override void OnAreaExit(Collider2D collision)
    {
        collision.gameObject.GetComponent<IMovable>().RemoveMovementSpeedModifier(movementSlowEffect);

        collision.gameObject.GetComponent<IDamager>().RemoveAttackSpeedModifier(attackSlowEffect);
       //Debug.Log("Return to normal speed!");
    }

}
