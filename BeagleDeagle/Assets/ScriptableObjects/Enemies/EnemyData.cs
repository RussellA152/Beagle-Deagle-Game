using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// We can use ScriptableObjects for enemies  that are very similar to each other
// This is useful if we have enemies that have the same behavior, and only different stats
// Ex. A fat zombie and regular zombie have the same behavior, but only different animations, movement speed, and health
[CreateAssetMenu(fileName = "NewEnemy", menuName = "ScriptableObjects/CharacterData/EnemyData/RegularEnemy")]
public class EnemyData : CharacterData
{
    public int attackDamage;

    public float attackCooldown;

    [Header("Line of Sight Values")]
    public float attackRange; // how close does this enemy need to be to its target to do an attack?
    public float chaseRange; // how close does this enemy need to be to its target to chase them?

    [Header("Who to Attack?")]
    public LayerMask attackLayer; // what kind of layer does this enemy attack? (ex. Player)
    [Header("Who to Chase?")]
    public LayerMask chaseLayer; // what kind of layer does this enemy follow? (ex. Player)

}