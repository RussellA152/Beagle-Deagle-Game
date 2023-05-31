using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGrenade", menuName = "ScriptableObjects/Grenade/SmokeGrenade")]
public class SmokeGrenade : GrenadeData
{
    [Range(0f, -1f)]
    [SerializeField]
    private float movementSlowAmount; // A positive value representing how much to decrease the enemy's movement speed by (%)

    [Range(0f, 1f)]
    [SerializeField]
    private float attackSlowAmount; // A positive value representing how much to increase the enemy's attack cooldown by (%)

    [SerializeField]
    private LayerMask layersToHit;

    private Dictionary<GameObject, int> overlappingEnemies = new Dictionary<GameObject, int>(); // Key: The enemy inside of the smoke grenade's trigger collider
                                                                                                // Value: The number of smoke grenade trigger colliders that the enemy is inside of

    private MovementSpeedModifier movementSlowEffect;
    private AttackSpeedModifier attackSlowEffect;

    private void OnEnable()
    {
        overlappingEnemies.Clear();

        movementSlowEffect = new MovementSpeedModifier(this.name, movementSlowAmount, true);
        attackSlowEffect = new AttackSpeedModifier(this.name, attackSlowAmount, true);
    }

    public override void Explode()
    {
        Debug.Log("Explode smoke grenade!");
    }

    public override void OnAreaEnter(Collider2D collision)
    {
        // When enemy enters the smoke grenade collider
        if (!overlappingEnemies.ContainsKey(collision.gameObject))
        {
            overlappingEnemies.Add(collision.gameObject, 0);
        }

        // Increment overlappingEnemies by 1
        overlappingEnemies[collision.gameObject]++;

        // Only apply slow effects for the first smoke grenade that the enemy walks into
        if (overlappingEnemies[collision.gameObject] == 1)
        {
            collision.gameObject.GetComponent<IMovable>().AddMovementSpeedModifier(movementSlowEffect);
            collision.gameObject.GetComponent<IDamager>().AddAttackSpeedModifier(attackSlowEffect);
        }
    }

    public override void OnAreaExit(Collider2D collision)
    {
        if (overlappingEnemies.ContainsKey(collision.gameObject))
        {
            // Decrement overlappingEnemies by 1
            overlappingEnemies[collision.gameObject]--;

            // When the enemy is no longer colliding with any smoke grenade trigger colliders, then remove the slow effects
            // This ensures that the slow effect 
            if (overlappingEnemies[collision.gameObject] == 0)
            {
                overlappingEnemies.Remove(collision.gameObject);
                collision.gameObject.GetComponent<IMovable>().RemoveMovementSpeedModifier(movementSlowEffect);
                collision.gameObject.GetComponent<IDamager>().RemoveAttackSpeedModifier(attackSlowEffect);
            }
        }
    }
}
