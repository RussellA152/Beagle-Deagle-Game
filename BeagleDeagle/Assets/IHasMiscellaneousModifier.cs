using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasMiscellaneousModifier
{
    ///-///////////////////////////////////////////////////////////
    /// Get a reference to a miscellaneous modifier list, then use it to fetch any variables needed
    /// Ex. An explosive bullet might require a bonus explosive radius.
    /// 
    public void GiveMiscellaneousModifierList(MiscellaneousModifierList miscellaneousModifierList);

    ///-///////////////////////////////////////////////////////////
    /// On disable or enable, set all bonuses received back to original values
    /// 
    public void ResetMiscellaneousBonuses();
}
