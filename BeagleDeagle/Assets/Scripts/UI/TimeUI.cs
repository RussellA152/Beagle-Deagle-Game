using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    

    private void Update()
    {
        DisplayTime();
    }

    private void DisplayTime()
    {
        int minutes = Mathf.FloorToInt(GameTimeManager.Instance.ElapsedTime / 60f);
        int seconds = Mathf.FloorToInt(GameTimeManager.Instance.ElapsedTime % 60f);

        string formattedTime = $"{minutes:D}:{seconds:00}";
        timeText.text = formattedTime;
    }
}
