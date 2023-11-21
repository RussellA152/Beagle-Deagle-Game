using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-///////////////////////////////////////////////////////////
/// A series of significant events involving player currency like xp and money. Events will occur when player gains or loses any xp or money.
/// 
[CreateAssetMenu(menuName = "GameEvent/CurrencyEvents")]
public class CurrencyEvents : ScriptableObject
{
    public event Action<int> givePlayerXp;
    
    public event Action<int> givePlayerMoney; 
    
    public void InvokeGiveXp(int xpAmount)
    {
        givePlayerXp?.Invoke(xpAmount);
    }
    
    public void InvokeGiveMoney(int moneyAmount)
    {
        givePlayerMoney?.Invoke(moneyAmount);
    }
}
