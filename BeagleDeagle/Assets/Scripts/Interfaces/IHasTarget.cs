using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasTarget
{
    ///-///////////////////////////////////////////////////////////
    /// Set the AI to have a new target to follow or attack (for enemies)
    /// 
    public void SetNewTarget(Transform newTarget);
}
