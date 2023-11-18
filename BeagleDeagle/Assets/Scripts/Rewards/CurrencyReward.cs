using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class CurrencyReward
{
    [Range(0f, 10000f)] public int xpAmount;
    
    [Range(0f, 10000f)] public int moneyAmount;

    public CurrencyReward(int xp, int money)
    {
        xpAmount = xp;
        moneyAmount = money;
    }
}
