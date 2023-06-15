using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewUtility", menuName = "ScriptableObjects/Ability/Utility")]
public abstract class UtilityAbilityData : AbilityData
{
    public int maxUses; // How many times can this ability be used?

    public abstract void ActivateUtility(ObjectPooler objectPool, GameObject player);

    // Both MightyFoot and Smoke Grenades spawn in the direction the player is facing (in slightly different ways)
    // This function does not add any force or velocity to the utility, it only places the utility at the player's direction
    public abstract void SpawnAtPlayerDirection(GameObject objectToSpawn, GameObject player);

}
