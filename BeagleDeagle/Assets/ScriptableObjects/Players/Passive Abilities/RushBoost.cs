using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "ScriptableObjects/Ability/Passive/RushBoost")]
public class RushBoost : PassiveAbilityData
{
    [Range(0f, 1f)]
    [SerializeField]
    private float speedBoost; // how much faster (%) should the player start running?

    [Range(0f, 10f)]
    [SerializeField]
    private float minimumTimeRequired;

    private bool speedIncreased;

    private IModifier modifierScript;

    private Gun gunScript;


    private void OnEnable()
    {
        speedIncreased = false;

        //playerEvents.playerObtainedNewWeaponEvent += UpdateScriptableObject;

    }

    public override void ActivatePassive(GameObject player)
    {
        // fetch gun script and cache it (Gun is a child in the Player gameobject)
        if (gunScript == null)
            gunScript = player.GetComponentInChildren<Gun>();

        // fetch movement script and cache it
        if (modifierScript == null)
            modifierScript = player.GetComponent<IModifier>();

        // if player doesn't currently have speed boost, and they haven't shot their gun in a while..
        // then give player a small speed increase
        if (!speedIncreased && gunScript.ReturnLastTimeShot() >= minimumTimeRequired)
        {
            speedIncreased = true;

            modifierScript.ModifyMovementSpeedModifier(speedBoost);
           
        }
        // otherwise, if the player did receive a speed increase, and they started shooting...
        // revert speed back to what it was
        else if (speedIncreased && gunScript.ReturnLastTimeShot() < minimumTimeRequired)
        {
            speedIncreased = false;

            modifierScript.ModifyMovementSpeedModifier(-speedBoost);
        }
    }
    //public void UpdateScriptableObject(GunData scriptableObject)
    //{
    //    currentWeaponData = scriptableObject;
    //}
}
