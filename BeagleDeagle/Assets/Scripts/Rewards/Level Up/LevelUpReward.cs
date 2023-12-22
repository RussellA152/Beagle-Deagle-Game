using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


///-///////////////////////////////////////////////////////////
/// When the player reaches a certain level, they will be given a certain scriptableObject 
/// as an upgrade to a gun, utility ability, or ultimate ability.
/// 
public abstract class LevelUpReward: IHasDescription
{
    [Range(1, 20),Tooltip("What level is the reward given at?")]
    public int LevelGiven;
    
    [Tooltip("Will this reward be one of many other choices when the player reaches the level requirement?")]
    public bool IsChosen;

    public Image Icon;

    public abstract string GetRewardName();
    public abstract void GiveDataToPlayer(GameObject recipientGameObject);

    public abstract string GetDescription();

}


