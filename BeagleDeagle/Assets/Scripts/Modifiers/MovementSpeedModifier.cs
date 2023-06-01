[System.Serializable]
public class MovementSpeedModifier: Modifier
{
    public float bonusMovementSpeed;
    public MovementSpeedModifier(string name, float movementSpeed)
    {
        modifierName = name;
        bonusMovementSpeed = movementSpeed;
        
    }
}
