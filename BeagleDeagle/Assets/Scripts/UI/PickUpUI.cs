using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpUI : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;

    [SerializeField] private TMP_Text textBox;

    [SerializeField, Range(1f, 5f)] 
    private float durationOfText;

    private bool _textIsCurrentlyDisplayed;

    private void OnEnable()
    {
        playerEvents.displayPlayerPickUpDescription += DisplayPickUpText;

        // Hide text on enable
        textBox.text = string.Empty;
        _textIsCurrentlyDisplayed = false;
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
        StartCoroutine(DisplayPickUpForDuration(description));
    }

    private IEnumerator DisplayPickUpForDuration(string description)
    {
        while (_textIsCurrentlyDisplayed)
            yield return null;
        
        textBox.text = description;

        _textIsCurrentlyDisplayed = true;
        
        yield return new WaitForSeconds(durationOfText);

        textBox.text = string.Empty;

        _textIsCurrentlyDisplayed = false;


    }
}
