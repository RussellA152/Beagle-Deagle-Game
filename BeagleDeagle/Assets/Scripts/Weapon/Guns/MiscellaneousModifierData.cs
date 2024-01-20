using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMiscellaneousStats", menuName = "ScriptableObjects/Stat Modifiers/Miscellaneous")]
public class MiscellaneousModifierData : ScriptableObject, IHasDescription
{
    public ExplosiveRadiusModifier explosiveRadiusModifier;

    public AreaOfEffectRadiusModifier aoeRadiusModifier;

    [SerializeField, Space(10), TextArea(2,3)] 
    private string description;
    
    public string GetDescription()
    {
        return description;
    }
}
