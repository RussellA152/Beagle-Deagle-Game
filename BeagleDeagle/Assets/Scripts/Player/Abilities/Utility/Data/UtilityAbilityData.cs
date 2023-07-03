using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewUtility", menuName = "ScriptableObjects/Ability/Utility")]
public abstract class UtilityAbilityData : AbilityData
{
    [Range(0,20)]
    public int maxUses; // How many times can this ability be used?
    

}
