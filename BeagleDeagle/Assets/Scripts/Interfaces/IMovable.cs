using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    public void AllowMovement(bool boolean);
    
    public void AddMovementSpeedModifier(MovementSpeedModifier modifierToAdd);

    public void RemoveMovementSpeedModifier(MovementSpeedModifier modifierToRemove);
}
