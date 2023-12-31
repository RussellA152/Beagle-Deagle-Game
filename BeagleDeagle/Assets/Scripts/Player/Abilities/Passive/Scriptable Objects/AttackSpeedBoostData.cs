using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackSpeedBoost", menuName = "ScriptableObjects/Stat Modifiers/Attack Speed Boost")]
public class AttackSpeedBoostData : ScriptableObject
{
    public AttackSpeedModifier attackSpeedModifier;
}
