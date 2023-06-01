using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "ScriptableObjects/Ability/Passive/RushBoost")]
// Give the player a speed boost when they are not shooting for a few seconds.
// Remove the boost the moment they start shooting.
public class RushBoost : PassiveAbilityData
{
    [Range(0f, 1f)]
    [SerializeField]
    private float speedBoost; // how much faster (%) should the player start running?

    [Range(0f, 10f)]
    [SerializeField]
    private float minimumTimeRequired;

    private bool speedIncreased;

    private IMovable movementScript;

    private MovementSpeedModifier movementSpeedModifier;

    private Gun gunScript;


    private void OnEnable()
    {
        speedIncreased = false;

        movementSpeedModifier = new MovementSpeedModifier(this.name, speedBoost);

    }

    public override void ActivatePassive(GameObject player)
    {
        // Fetch gun script and cache it (Gun is a child in the Player gameobject)
        if (gunScript == null)
            gunScript = player.GetComponentInChildren<Gun>();

        // fetch movement script and cache it
        if (movementScript == null)
            movementScript = player.GetComponent<IMovable>();

        // If player doesn't currently have speed boost, and they haven't shot their gun in a while..
        // Then give player a small speed increase
        if (!speedIncreased && gunScript.ReturnLastTimeShot() >= minimumTimeRequired)
        {
            speedIncreased = true;

            movementScript.AddMovementSpeedModifier(movementSpeedModifier);
           
        }
        // Otherwise, if the player did receive a speed increase, and they started shooting...
        // Then revert speed back to what it was
        else if (speedIncreased && gunScript.ReturnLastTimeShot() < minimumTimeRequired)
        {
            speedIncreased = false;

            movementScript.RemoveMovementSpeedModifier(movementSpeedModifier);
        }
    }
}
