using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "ScriptableObjects/Ability/Passive/HealthBoost")]
public class HealthBoost : PassiveAbilityData
{
    public float increaseAmount;

    public override void ActivatePassive(GameObject player)
    {
        player.gameObject.GetComponent<IHealth>().ModifyMaxHealth(increaseAmount);
    }
}
