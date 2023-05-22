using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent/PlayerEvents")]
public class PlayerEventSO : ScriptableObject
{
    public event Action<float> currentHealthChangedEvent;

    public event Action<float> maxHealthChangedEvent;

    //public event Action<float> lastTimeShotEvent;

    public event Action<GunData> playerObtainedNewWeaponEvent;

    public event Action<PlayerData> playerObtainedNewStatsEvent;

    public void InvokeMaxHealthEvent(float maxHealth)
    {
        if(maxHealthChangedEvent != null)
        {
            maxHealthChangedEvent(maxHealth);
        }
    }
    public void InvokeCurrentHealthEvent(float currentHealth)
    {
        if (currentHealthChangedEvent != null)
        {
            currentHealthChangedEvent(currentHealth);
        }
    }

    public void InvokeNewWeaponEvent(GunData newWeaponData)
    {
        if(playerObtainedNewWeaponEvent != null)
        {
            playerObtainedNewWeaponEvent(newWeaponData);
        }
    }

    public void InvokeNewStatsEvent(PlayerData newPlayerData)
    {
        if (playerObtainedNewStatsEvent != null)
        {
            playerObtainedNewStatsEvent(newPlayerData);
        }
    }

    //public void InvokeLastTimeShotEvent(float time)
    //{
    //    if (lastTimeShotEvent != null)
    //    {
    //        lastTimeShotEvent(time);
    //    }
    //}
}
