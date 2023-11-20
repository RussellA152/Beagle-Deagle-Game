using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text gameTimeText;
    [SerializeField] private TMP_Text nextObjectiveTimeText;
    
    private void Update()
    {
        DisplayTime();
        DisplayNextObjectiveTimer();
    }

    ///-///////////////////////////////////////////////////////////
    /// Display how much time has past by while playing the level
    /// 
    private void DisplayTime()
    {
        int minutes = Mathf.FloorToInt(GameTimeManager.Instance.ElapsedTime / 60f);
        int seconds = Mathf.FloorToInt(GameTimeManager.Instance.ElapsedTime % 60f);

        string formattedTime = $"{minutes:D}:{seconds:00}";
        gameTimeText.text = formattedTime;
    }

    ///-///////////////////////////////////////////////////////////
    /// Display how much time is left until the next map objective begins.
    /// 
    private void DisplayNextObjectiveTimer()
    {
        int seconds = Mathf.FloorToInt(MapObjectiveManager.instance.GetNextObjectiveTime() % 60f);

        string formattedTime = $"{seconds:00}";
        nextObjectiveTimeText.text = formattedTime;
    }
}
