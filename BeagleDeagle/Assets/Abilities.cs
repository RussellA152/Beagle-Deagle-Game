using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    // a list of the player's passive abilities
    [SerializeField]
    private List<PassiveAbilityData> passives = new List<PassiveAbilityData>();

    public void ActivateAllPassives()
    {
        foreach (PassiveAbilityData passive in passives)
        {
            if (passive.activationType == PassiveAbilityData.PassiveActivationType.Once)
                passive.ActivatePassive(gameObject);

            else if (passive.activationType == PassiveAbilityData.PassiveActivationType.Continuous)
                StartCoroutine(StartContinuousPassive(passive));
        }
    }
    // Continuous passives will activate many times, so we use a coroutine
    private IEnumerator StartContinuousPassive(PassiveAbilityData passive)
    {
        while (true)
        {
            passive.ActivatePassive(gameObject);
            yield return null;
        }
    }
}

