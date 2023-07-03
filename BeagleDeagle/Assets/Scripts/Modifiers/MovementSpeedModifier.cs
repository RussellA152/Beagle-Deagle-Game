using UnityEngine;

[System.Serializable]
public class MovementSpeedModifier: Modifier
{
    [Range(-1f, 1f)]
    public float bonusMovementSpeed;
    
    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to a entity's movement speed.
    /// 
    public MovementSpeedModifier(string name, float movementSpeed)
    {
        modifierName = name;
        bonusMovementSpeed = movementSpeed;
        
    }
}
