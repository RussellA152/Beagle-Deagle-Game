using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpUI : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;

    [SerializeField] private TMP_Text textBox;

    private void OnEnable()
    {
        playerEvents.displayPlayerPickUpDescription += DisplayPickUpText;

        // Hide text on enable
        textBox.text = string.Empty;
    }

    private void OnDisable()
    {
        playerEvents.displayPlayerPickUpDescription -= DisplayPickUpText;
    }

    ///-///////////////////////////////////////////////////////////
    /// When player retrieves a pick up, show a message on the screen that describes what they just got.
    /// Ex. Player retrieves a speed boost pick up, and sees "Increased Movement and Attack Speed!"
    private void DisplayPickUpText(string description)
    {
        textBox.text = description;
    }
}
