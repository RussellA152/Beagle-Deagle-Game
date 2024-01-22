using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewReloadSpeedBoost", menuName = "ScriptableObjects/Stat Modifiers/Reload Speed Boost")]
public class ReloadSpeedBoostData : ScriptableObject
{
    // What is the reload speed boost modification applied to the player's weapon?
    public ReloadSpeedModifier reloadSpeedModifier;
}
