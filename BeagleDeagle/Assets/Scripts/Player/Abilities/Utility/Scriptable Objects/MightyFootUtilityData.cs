using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewMightyFoot", menuName = "ScriptableObjects/Ability/Utility/MightyFoot")]
public class MightyFootUtilityData : UtilityAbilityData
{
    [RestrictedPrefab(typeof(MightyFootBullet))]
    // The mighty foot to spawn
    public GameObject mightyFootPrefab;

    // The data the mighty foot will use 
    public MightyFootBulletData mightyFootData;
    
    
    // The data of the status effects the mighty foot will use ( * MUST BE COMPATIBLE WITH PREFAB * )
    public StatusEffectTypes statusEffects;
    
    [Header("Offset From Player Position")]
    public Vector2 offset; // Offset applied to Mighty Foot projectile when this ability is activated
}
