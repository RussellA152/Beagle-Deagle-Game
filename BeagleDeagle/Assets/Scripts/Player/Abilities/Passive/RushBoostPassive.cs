using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "ScriptableObjects/Ability/Passive/RushBoost")]
// Give the player a speed boost when they are not shooting for a few seconds.
// Remove the boost the moment they start shooting.
public class RushBoostPassive : PassiveAbilityData
{
    [Range(0f, 1f)]
    [SerializeField]
    private float speedBoost; // how much faster (%) should the player start running?

    [Range(0f, 10f)]
    [SerializeField]
    private float minimumTimeRequired;

    private MovementSpeedModifier _movementSpeedModifier;


    private void OnEnable()
    {
        _movementSpeedModifier = new MovementSpeedModifier(this.name, speedBoost);

    }
    

    public override IEnumerator ActivatePassive(GameObject player)
    {
        Gun gunScript = player.GetComponentInChildren<Gun>();
        
        IMovable movementScript = player.GetComponent<IMovable>();

        bool speedIncreased = false;
        //Debug.Log("Get component once!");
        
        while (true)
        {
            //Debug.Log("while loop!");
            if (!speedIncreased && gunScript.ReturnLastTimeShot() >= minimumTimeRequired)
            {
                speedIncreased = true;

                movementScript.AddMovementSpeedModifier(_movementSpeedModifier);
           
            }
            // Otherwise, if the player did receive a speed increase, and they started shooting...
            // Then revert speed back to what it was
            else if (speedIncreased && gunScript.ReturnLastTimeShot() < minimumTimeRequired)
            {
                speedIncreased = false;

                movementScript.RemoveMovementSpeedModifier(_movementSpeedModifier);
            }
            
            yield return null;
        }
    }

    public override void RemovePassive(GameObject player)
    {
        IMovable movementScript = player.GetComponent<IMovable>();

        movementScript.RemoveMovementSpeedModifier(_movementSpeedModifier);
        
    }
}
