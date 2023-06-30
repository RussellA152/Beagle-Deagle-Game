using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeUltimateAbility : UltimateAbility<NukeUltimateData>
{
    protected override void UltimateAction(GameObject player)
    {
        StartCoroutine(Cooldown());
        StartCoroutine(CountDownCooldown());
        
        Debug.Log("Spawn nuclear bomb!");

        GameObject nuclearBomb = Instantiate(ultimateData.prefab);

        nuclearBomb.SetActive(false);

        nuclearBomb.transform.position = player.transform.position;

        nuclearBomb.GetComponent<Nuke>().UpdateExplosiveData(ultimateData.nukeData);

        nuclearBomb.SetActive(true);
        

    }
}
