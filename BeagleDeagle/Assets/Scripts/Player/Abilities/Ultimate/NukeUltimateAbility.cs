using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeUltimateAbility : UltimateAbilityActivator
{
    protected override void UltimateAction(GameObject player)
    {
        StartCoroutine(UltimateCooldown());
        StartCoroutine(CountDownCooldown());
        
        Debug.Log("Spawn nuclear bomb!");

        // Spawn a nuke at the player's location
        GameObject nuclearBomb = Instantiate(ultimateData.nukePrefab);

        nuclearBomb.SetActive(false);

        nuclearBomb.transform.position = player.transform.position;

        // Give the nuke the data it needs
        nuclearBomb.GetComponent<Nuke>().UpdateScriptableObject(ultimateData.nukeData);

        nuclearBomb.SetActive(true);
        

    }
}
