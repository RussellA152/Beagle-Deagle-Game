using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text gameTimeText;

    private void Update()
    {
        DisplayTime();
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
    
}
