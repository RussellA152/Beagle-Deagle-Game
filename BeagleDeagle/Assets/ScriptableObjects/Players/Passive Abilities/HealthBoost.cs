using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "ScriptableObjects/Ability/Passive/HealthBoost")]
public class HealthBoost : PassiveAbilityData
{
    [Range(0f, 1f)]
    public float increaseAmount;

    public override void ActivatePassive(GameObject player)
    {
        //player.gameObject.GetComponent<IHealth>().ModifyMaxHealth(increaseAmount);
        player.gameObject.GetComponent<IModifier>().ModifyMaxHealthModifier(increaseAmount);
    }
}
