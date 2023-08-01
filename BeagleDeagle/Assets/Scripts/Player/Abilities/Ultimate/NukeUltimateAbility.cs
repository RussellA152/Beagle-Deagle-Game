using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeUltimateAbility : UltimateAbility<NukeUltimateData>
{
    
    protected override void UltimateAction(GameObject player)
    {
        StartCooldowns();
        
        Debug.Log("Spawn nuclear bomb!");

        // Spawn a nuke at the player's location
        GameObject nuclearBomb = Instantiate(ultimateData.nukePrefab);

        nuclearBomb.SetActive(false);

        nuclearBomb.transform.position = player.transform.position;

        IExplosiveUpdatable explosiveScript = nuclearBomb.GetComponent<IExplosiveUpdatable>();

        // Give the nuke the data it needs
        explosiveScript.UpdateScriptableObject(ultimateData.nukeData);
        
        explosiveScript.SetDamage(ultimateData.abilityDamage);
        explosiveScript.SetDuration(ultimateData.duration);

        nuclearBomb.GetComponent<StatusEffect<DamageOverTimeData>>().UpdateScriptableObject(ultimateData.damageOverTimeData);
        
        nuclearBomb.SetActive(true);
        
        explosiveScript.Activate(transform.position);

    }
}
