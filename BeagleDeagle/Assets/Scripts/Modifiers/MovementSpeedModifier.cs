[System.Serializable]
public class MovementSpeedModifier: Modifier
{
    public float bonusMovementSpeed;
    public MovementSpeedModifier(string name, float movementSpeed, bool isAppliedOnTriggerEnter)
    {
        modifierName = name;
        bonusMovementSpeed = movementSpeed;
        
    }
}
