using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUtility", menuName = "ScriptableObjects/Ability/Utility")]
public abstract class UtilityAbilityData : ScriptableObject
{
    public float cooldown; // how long will it take for the player to be able to use this again?

    public int maxUses; // how many times can this ability be used

    public abstract void ActivateUtility(ObjectPooler objectPool, GameObject player);

}
