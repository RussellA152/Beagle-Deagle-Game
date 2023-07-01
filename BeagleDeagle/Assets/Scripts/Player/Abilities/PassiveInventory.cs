using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveInventory : MonoBehaviour
{
    [SerializeField]
    private PlayerEvents playerEvents;

    // a list of the player's passive abilities
    [SerializeField]
    private List<PassiveAbilityData> passives = new List<PassiveAbilityData>();

    private void Start()
    {
        ActivateAllPassives();

        //StartCoroutine(TestRemovePassive());

    }

    private void ActivateAllPassives()
    {
        foreach (PassiveAbilityData passive in passives)
        {
            Debug.Log(passive.name);
            
            StartCoroutine(passive.ActivatePassive(gameObject));
        }
    }

    private void RemovePassiveFromInventory(PassiveAbilityData passive)
    {
        passive.RemovePassive(gameObject);
        
        passives.Remove(passive);
    }

    // Testing the removal of passives
    IEnumerator TestRemovePassive()
    {
        int count = passives.Count;
        
        yield return new WaitForSeconds(5f);
        
        for(int i = 0; i < count; i++)
        {
            passives[0].RemovePassive(gameObject);
            passives.Remove(passives[0]);
        }
    }
}

