using System.Collections;
using System.Collections.Generic;
using UnityEngine;



///-///////////////////////////////////////////////////////////
/// Basic stats for enemies such as damage, cooldowns, and line of sight.
/// 
public class EnemyData : CharacterData
{
    [Header("Damage")]
    // How damage damage does this enemy apply to their target?
    [Range(-1000f, 1000f)] public float attackDamage;
    // How long does it take (seconds) for this enemy to attack again?
    [Range(0.1f, 30f)] public float attackCooldown;

    [Header("Line of Sight Values")]
    // How close does this enemy need to be to its target to do an attack?
    [Range(0.1f, 100f)] public float attackRange;
    // How close does this enemy need to be to its target to chase them?
    [Range(0.1f, 150f)] public float chaseRange;

    [Header("Who to Attack?")]
    public LayerMask attackLayer; // What kind of layer does this enemy attack? (ex. Player)
    [Header("Who to Chase?")]
    
    public LayerMask chaseLayer; // What kind of layer does this enemy follow? (ex. Player)
    
    [Range(0f, 1000f)]
    public float xpOnDeath;

}
