using UnityEngine;

[CreateAssetMenu(fileName = "NewSpeedBoost", menuName = "ScriptableObjects/Stat Modifiers/Movement Speed Boost")]

public class MovementSpeedBoostData : ScriptableObject
{
    // What is the speed boost modification applied to the player?
    public MovementSpeedModifier movementSpeedModifier;

}
