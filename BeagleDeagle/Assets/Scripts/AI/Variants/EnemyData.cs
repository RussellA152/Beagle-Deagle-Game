using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// We can use ScriptableObjects for enemies  that are very similar to each other
// This is useful if we have enemies that have the same behavior, and only different stats
// Ex. A fat zombie and regular zombie have the same behavior, but only different animations, movement speed, and health
[CreateAssetMenu(fileName = "NewEnemy", menuName = "ScriptableObjects/CharacterData/EnemyData/RegularEnemy")]
public class EnemyData : CharacterData
{
    [Header("Damage")]
    public int attackDamage; // How damage damage does this enemy apply to their target?
    public float attackCooldown; // How long does it take (seconds) for this enemy to attack again?

    [Header("Line of Sight Values")]
    public float attackRange; // How close does this enemy need to be to its target to do an attack?
    public float chaseRange; // How close does this enemy need to be to its target to chase them?

    [Header("Who to Attack?")]
    public LayerMask attackLayer; // What kind of layer does this enemy attack? (ex. Player)
    [Header("Who to Chase?")]
    public LayerMask chaseLayer; // What kind of layer does this enemy follow? (ex. Player)

}
