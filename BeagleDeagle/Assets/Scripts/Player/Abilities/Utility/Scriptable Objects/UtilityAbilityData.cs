using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UtilityAbilityData : AbilityData
{
    [Range(0,20)]
    public int maxUses; // How many times can this ability be used?

}
