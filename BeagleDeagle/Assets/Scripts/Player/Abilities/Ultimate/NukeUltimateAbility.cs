using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeUltimateAbility : MonoBehaviour
{
    [SerializeField] 
    private UltimateActivator ultimateActivator;
    
    [SerializeField] 
    private NukeUltimateData ultimateData;

    public void UltimateAction(GameObject player)
    {
        ultimateActivator.StartCooldowns();
        
        Debug.Log("Spawn nuclear bomb!");

        // Spawn a nuke at the player's location
        GameObject nuclearBomb = Instantiate(ultimateData.nukePrefab);

        nuclearBomb.SetActive(false);

        nuclearBomb.transform.position = player.transform.position;

        // Give the nuke the data it needs
        nuclearBomb.GetComponent<Nuke>().UpdateScriptableObject(ultimateData.nukeData);
        
        nuclearBomb.GetComponent<StatusEffect<DamageOverTimeData>>().UpdateScriptableObject(ultimateData.damageOverTimeData);

        nuclearBomb.SetActive(true);

    }
}
