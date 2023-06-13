using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewUtility", menuName = "ScriptableObjects/Ability/Utility")]
public abstract class UtilityAbilityData : AbilityData
{
    //public float abilityDamage;

    //[Header("Usage")]
    //public float cooldown; // How long will it take for the player to be able to use this again?
    public int maxUses; // How many times can this ability be used?

    //public float duration; // How long smoke grenade lasts for & life time for mighty foot bullet

    public abstract void ActivateUtility(ObjectPooler objectPool, GameObject player);

    // Both MightyFoot and Smoke Grenades spawn in the direction the player is facing (in slightly different ways)
    // This function does not add any force or velocity to the utility, it only places the utility at the player's direction
    public abstract GameObject SpawnAtPlayerDirection(GameObject objectToSpawn, GameObject player);

}
