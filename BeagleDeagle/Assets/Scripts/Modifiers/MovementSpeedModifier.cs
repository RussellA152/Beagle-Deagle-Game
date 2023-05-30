
[System.Serializable]
public class MovementSpeedModifier
{
    public string modifierName;
    public float bonusMovementSpeed;
    public bool appliedOnTriggerEnter;

    public MovementSpeedModifier(string name, float movementSpeed, bool isAppliedOnTriggerEnter)
    {
        modifierName = name;
        bonusMovementSpeed = movementSpeed;
        appliedOnTriggerEnter = isAppliedOnTriggerEnter;
    }
}
