using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "ScriptableObjects/Ability/Passive")]
public abstract class PassiveAbilityData : ScriptableObject
{
    public PassiveActivationType activationType;

    public enum PassiveActivationType
    {
       Once, // should this passive occur once for the player? (Ex. Increased max health for player)

       Continuous // should this passive occur many times? (Ex. Increase movement speed when not shooting for a few seconds)
    }

    public abstract void ActivatePassive(GameObject player);

}
