using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "ScriptableObjects/Ability/Passive")]
public abstract class PassiveAbilityData : ScriptableObject
{
    public abstract IEnumerator ActivatePassive(GameObject player);
    
    public abstract void RemovePassive(GameObject player);

}
