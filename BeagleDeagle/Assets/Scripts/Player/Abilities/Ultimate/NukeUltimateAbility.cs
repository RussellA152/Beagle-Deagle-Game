using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeUltimateAbility : UltimateAbility<NukeUltimateData>
{
    
    protected override void UltimateAction()
    {
        StartCooldown();

        // Spawn a nuke at the player's location
        GameObject nuclearBomb = Instantiate(ultimateData.nukePrefab);

        nuclearBomb.SetActive(false);

        nuclearBomb.transform.position = gameObject.transform.position;

        IExplosiveUpdatable explosiveScript = nuclearBomb.GetComponent<IExplosiveUpdatable>();

        // Give the nuke the data it needs
        explosiveScript.UpdateScriptableObject(ultimateData.nukeData);
        
        // Set the damage of the nuke blast and the duration the nuke lingers for
        explosiveScript.SetDamage(ultimateData.abilityDamage);
        explosiveScript.SetDuration(ultimateData.duration);

        foreach (IStatusEffect statusEffect in nuclearBomb.GetComponents<IStatusEffect>())
        {
            statusEffect.UpdateWeaponType(ultimateData.statusEffects);
        }
        
        nuclearBomb.SetActive(true);
        
        // Play nuke animation
        nuclearBomb.GetComponentInChildren<NukeShadowAnimation>().PlayNukeShadowAnimation();
        
        explosiveScript.Activate(transform.position);

    }
}
