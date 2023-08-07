using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    ///-///////////////////////////////////////////////////////////
    /// Enable or disable movement of an entity that can move around.
    /// 
    public void AllowMovement(bool boolean);
    
    public void AddMovementSpeedModifier(MovementSpeedModifier modifierToAdd);

    public void RemoveMovementSpeedModifier(MovementSpeedModifier modifierToRemove);
}
