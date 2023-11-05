using UnityEngine;

[System.Serializable]
public class MovementSpeedModifier: Modifier
{
    [Range(-1f, 1f)]
    public float bonusMovementSpeed;
    
    // The particle effect that will play from this modifier, if it has one
    [RestrictedPrefab(typeof(PoolableParticle))]
    public GameObject particleEffect;
    
    ///-///////////////////////////////////////////////////////////
    /// An increase or decrease applied to a entity's movement speed.
    /// 
    public MovementSpeedModifier(string name, float movementSpeed)
    {
        modifierName = name;
        bonusMovementSpeed = movementSpeed;
    }
}
