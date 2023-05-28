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
        IHealth playerHealth = player.GetComponent<IHealth>();


        if (playerHealth != null)
        {
            // Option 1
            playerHealth.AddMaxHealthModifier(new MaxHealthModifier(increaseAmount));

        }


        //// Option 2
        //IModifier modifierScript = player.GetComponent<IModifier>();

        //if(modifierScript != null)
        //{
        //    modifierScript.ModifyMaxHealthModifier(increaseAmount);
        //}      
    }
}

