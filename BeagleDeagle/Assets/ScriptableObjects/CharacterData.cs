using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// We can use ScriptableObjects for enemies  that are very similar to each other
// This is useful if we have enemies that have the same behavior, and only different stats
// Ex. A fat zombie and regular zombie have the same behavior, but only different animations, movement speed, and health
[CreateAssetMenu(fileName = "NewEnemy", menuName = "ScriptableObjects/CharacterData/EnemyData/RegularEnemy")]
public class CharacterData : ScriptableObject
{
    public RuntimeAnimatorController animatorController;

    [Range(1f, 2500f)]
    public float maxHealth;

    [Range(0f, 50f)]
    public float movementSpeed;


}
