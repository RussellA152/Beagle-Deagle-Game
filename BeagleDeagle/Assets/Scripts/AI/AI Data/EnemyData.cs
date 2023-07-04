using System.Collections;
using System.Collections.Generic;
using UnityEngine;



///-///////////////////////////////////////////////////////////
/// Basic stats for enemies such as damage, cooldowns, and line of sight.
/// 
public class EnemyData : CharacterData
{
    [Header("Damage")]
    [Range(-1000f, 1000f)]
    // How damage damage does this enemy apply to their target?
    public float attackDamage;
    // How long does it take (seconds) for this enemy to attack again?
    public float attackCooldown;

    [Header("Line of Sight Values")]
    // How close does this enemy need to be to its target to do an attack?
    public float attackRange;
    // How close does this enemy need to be to its target to chase them?
    public float chaseRange;

    [Header("Who to Attack?")]
    public LayerMask attackLayer; // What kind of layer does this enemy attack? (ex. Player)
    [Header("Who to Chase?")]
    public LayerMask chaseLayer; // What kind of layer does this enemy follow? (ex. Player)

}
