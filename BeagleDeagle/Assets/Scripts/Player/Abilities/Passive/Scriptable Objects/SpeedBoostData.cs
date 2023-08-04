using UnityEngine;

[CreateAssetMenu(fileName = "NewSpeedBoost", menuName = "ScriptableObjects/Ability/Passive/SpeedBoost")]
// Give the player a speed boost when they are not shooting for a few seconds.
// Remove the boost the moment they start shooting.
public class SpeedBoostData : ScriptableObject
{
    [Range(0f, 10f)]
    public float minimumTimeRequired;
    
    public MovementSpeedModifier movementSpeedModifier;

}
