using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

///-///////////////////////////////////////////////////////////
/// The health, movement, and animations for a player character.
/// 
[CreateAssetMenu(fileName = "NewPlayer", menuName = "ScriptableObjects/CharacterData/Player")]
public class PlayerData : ScriptableObject
{
    // Name to display in-game
    public string characterName;
    
    public RuntimeAnimatorController animatorController;

    [Header("Health")]
    [Range(1f, 2500f)] public float maxHealth;

    [Header("Movement")]
    [Range(0f, 100f)] public float movementSpeed;
    public Vector2 rollPower;
    [Range(0.1f, 30f)] public float rollCooldown;
}
