using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeUltimateAbility : UltimateAbility<NukeUltimateData>
{
    protected override void UltimateAction()
    {
        StartCooldown();

        // Spawn a nuke at the player's location
        GameObject nuclearBomb = Instantiate(UltimateAbilityData.nukePrefab);

        nuclearBomb.SetActive(false);

        IExplosiveUpdatable explosiveScript = nuclearBomb.GetComponent<IExplosiveUpdatable>();

        // Give the nuke the data it needs
        explosiveScript.UpdateScriptableObject(UltimateAbilityData.nukeData);
        
        // Set the damage of the nuke blast and the duration the nuke lingers for
        explosiveScript.SetDamage(UltimateAbilityData.abilityDamage * BonusUltimateDamage);
        explosiveScript.SetDuration(UltimateAbilityData.duration);

        foreach (IStatusEffect statusEffect in nuclearBomb.GetComponents<IStatusEffect>())
        {
            statusEffect.UpdateWeaponType(UltimateAbilityData.statusEffects);
        }
        
        // Give nuke any extra modifiers (ex. giving a nuke "bonusExplosiveRadius")
        foreach (IHasMiscellaneousModifier hasMiscellaneousModifier in nuclearBomb.GetComponents<IHasMiscellaneousModifier>())
        {
            hasMiscellaneousModifier.GiveMiscellaneousModifierList(MiscellaneousModifierList);
        }
        
        nuclearBomb.SetActive(true);
        
        // Play nuke animation
        nuclearBomb.GetComponentInChildren<NukeShadowAnimation>().PlayNukeShadowAnimation();
        
        explosiveScript.Activate(transform.position);

    }

    protected override void PlayActivationSound()
    {
        AudioClipPlayer.PlayForDurationAudioClip(UltimateAbilityData.activationSound, UltimateAbilityData.activationSoundVolume, UltimateAbilityData.nukeData.detonationTime);
    }
}
