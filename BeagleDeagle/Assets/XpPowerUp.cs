using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpPowerUp : PowerUp
{
    [SerializeField] private CurrencyEvents currencyEvents;
    [SerializeField] private CurrencyReward currencyReward;
    
    protected override void OnPickUp(GameObject receiverGameObject)
    {
        currencyEvents.InvokeGiveXp(currencyReward.xpAmount);
    }
}
