[System.Serializable]
public class MovementSpeedModifier: Modifier
{
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
