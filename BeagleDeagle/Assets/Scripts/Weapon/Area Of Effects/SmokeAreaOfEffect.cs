using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAreaOfEffect", menuName = "ScriptableObjects/Area of Effects/Slow Smoke")]
public class SmokeAreaOfEffect : AreaOfEffectData
{
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

    public override void OnAreaEnter(Collider2D targetCollider)
    {
        // When enemy enters the smoke grenade collider
        if (!overlappingEnemies.ContainsKey(targetCollider.gameObject))
        {
            overlappingEnemies.Add(targetCollider.gameObject, 0);
        }

        // Increment overlappingEnemies by 1
        overlappingEnemies[targetCollider.gameObject]++;

        // Only apply slow effects for the first smoke grenade that the enemy walks into
        if (overlappingEnemies[targetCollider.gameObject] == 1)
        {
            targetCollider.gameObject.GetComponent<IMovable>().AddMovementSpeedModifier(movementSlowEffect);
            targetCollider.gameObject.GetComponent<IDamager>().AddAttackSpeedModifier(attackSlowEffect);
        }

        // Deal small damage to enemies that enter?
    }

    public override void OnAreaExit(Collider2D targetCollider)
    {
        if (overlappingEnemies.ContainsKey(targetCollider.gameObject))
        {
            // Decrement overlappingEnemies by 1
            overlappingEnemies[targetCollider.gameObject]--;

            // When the enemy is no longer colliding with any smoke grenade trigger colliders, then remove the slow effects
            // This ensures that the slow effect 
            if (overlappingEnemies[targetCollider.gameObject] == 0)
            {
                overlappingEnemies.Remove(targetCollider.gameObject);
                targetCollider.gameObject.GetComponent<IMovable>().RemoveMovementSpeedModifier(movementSlowEffect);
                targetCollider.gameObject.GetComponent<IDamager>().RemoveAttackSpeedModifier(attackSlowEffect);
            }
        }
    }
}
