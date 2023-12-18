using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityData : ScriptableObject, IHasDescription
{
    public string abilityName;
    
    [Space(10), TextArea(2,3)]
    public string description;

    // What icon will this ability have on the HUD?
    public Sprite abilitySprite;
    
    [Range(0, 1000f)]
    public float abilityDamage;

    [Header("Usage"), Range(0f,240f)]
    public float cooldown; // How long will it take for the player to be able to use this again?

    [Range(0f,240f)]
    public float duration; // How long smoke grenade lasts for & life time for mighty foot bullet
    
    public string GetDescription()
    {
        return description;
    }
    
}
