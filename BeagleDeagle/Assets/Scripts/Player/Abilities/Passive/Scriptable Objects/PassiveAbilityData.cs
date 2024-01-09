using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "ScriptableObjects/Ability/Passives")]
public class PassiveAbilityData : ScriptableObject, IHasDescription
{
    [Space(10), TextArea(2,3)]
    public string passiveName;

    public Sprite abilityIcon;
    
    [Space(10), TextArea(2,3)]
    public string description;
    
    public GameObject gameObjectWithPassive;

    public string GetDescription()
    {
        return description;
    }
}
