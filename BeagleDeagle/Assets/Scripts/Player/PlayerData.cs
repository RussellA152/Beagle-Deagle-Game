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

    // What set of rewards does this character receive?
    [FormerlySerializedAs("rewardsList")] public LevelUpRewardList levelUpRewardsList;

    [Header("Health")]
    [Range(1f, 2500f)] public float maxHealth;

    public HealthRegenData healthRegenData;

    [Header("Movement")]
    [Range(0f, 100f)] public float movementSpeed;
    public Vector2 rollPower;
    [Range(0.1f, 30f)] public float rollCooldown;
    
    public AudioClip[] rollSounds;
    [Range(0.1f, 1f)]
    public float rollSoundVolume;

    [Header("Starting Weapon and Abilities")]
    public GunData gunData;
    public PassiveAbilityData passiveAbilityData;
    public UtilityAbilityData utilityAbilityData;
    public UltimateAbilityData ultimateAbilityData;

    [Space(20)] 
    // How much xp does the player require at each level? (Ex. Level 2 requires 'x' amount of xp)
    public int[] xpNeededPerLevel;

    [Header("Theme Music")]
    // What music plays when this character loads into a level?
    public AudioClip characterTheme;

    [Range(0.05f, 1f)]
    public float themeVolume;

}
