using UnityEngine;

[CreateAssetMenu(fileName = "NewSpeedBoost", menuName = "ScriptableObjects/Ability/Passive/SpeedBoost")]

public class SpeedBoostData : ScriptableObject
{
    // How long must the player not attack for, to receive a speed boost?
    [Range(0f, 10f)]
    public float minimumTimeRequired;
    
    // What is the speed boost modification applied to the player?
    public MovementSpeedModifier movementSpeedModifier;

}
