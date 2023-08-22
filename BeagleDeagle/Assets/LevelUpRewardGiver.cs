using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpRewardGiver : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;

    [SerializeField] private LevelUpReward levelUpRewards;

    private GameObject _playerGameObject;
    
    // Scripts that the Giver will give scriptableObjects to
    private IGunDataUpdatable _gunScript;
    private IUtilityUpdatable _utilityScript;
    private IUltimateUpdatable _ultimateScript;

    private void OnEnable()
    {
        playerEvents.givePlayerGameObject += FindPlayer;
    }

    private void Start()
    {
        // Debug.Log(levelUpRewards.dataToGive[0].GetType());
        //
        // Debug.Log(_gunScript.);
        //
        // if (levelUpRewards.dataToGive[0] is GunData)
        // {
        //     Debug.Log("Reward is a GunData or derived type");
        // }
        // else
        // {
        //     Debug.Log("Reward is not a GunData or derived type");
        // }
    }

    private void FindPlayer(GameObject pGameObject)
    {
        _playerGameObject = pGameObject;

        _gunScript = _playerGameObject.GetComponent<IGunDataUpdatable>();

        _utilityScript = _playerGameObject.GetComponent<IUtilityUpdatable>();

        _ultimateScript = _playerGameObject.GetComponent<IUltimateUpdatable>();
    }
}
