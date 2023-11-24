using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] private GameEvents gameEvents;
    
    [SerializeField] private TMP_Text objectiveTextBox;
    
    [SerializeField] private RectTransform objectiveTimerRectTransform;

    private MapObjective _currentMapObjective;

    private bool _shouldDisplay = false;

    private float _targetWidth = 0f; // Adjust this to the desired final width
    private float _originalWidth; // Width of the timer before shrinking

    private void Start()
    {
        objectiveTextBox.gameObject.SetActive(false);
        objectiveTimerRectTransform.gameObject.SetActive(false);

        _originalWidth = objectiveTimerRectTransform.sizeDelta.x;
    }

    private void OnEnable()
    {
        gameEvents.onMapObjectiveBegin += StartDisplay;

        gameEvents.onMapObjectiveExited += Shrink;

        gameEvents.onMapObjectiveEntered += StopShrink;
        
        gameEvents.onMapObjectiveEnded += StopDisplay;
    }

    private void OnDisable()
    {
        gameEvents.onMapObjectiveBegin -= StartDisplay;
      
        gameEvents.onMapObjectiveExited -= Shrink;

        gameEvents.onMapObjectiveEntered -= StopShrink;
        
        gameEvents.onMapObjectiveEnded -= StopDisplay;
    }

    private void Update()
    {
        // Continuously update the objective's description
        if(_shouldDisplay)
            objectiveTextBox.text = _currentMapObjective.GetObjectiveDescription();
    }

    ///-///////////////////////////////////////////////////////////
    /// Allow the display of the map objective's description and timer
    /// 
    private void StartDisplay(MapObjective mapObjective)
    {
        _currentMapObjective = mapObjective;

        _shouldDisplay = true;
        
        objectiveTextBox.text = _currentMapObjective.GetObjectiveDescription();
        
        objectiveTextBox.gameObject.SetActive(true);
    }

    ///-///////////////////////////////////////////////////////////
    /// Stop showing the map objective's description and timer
    /// 
    private void StopDisplay(MapObjective mapObjective)
    {
        _shouldDisplay = false;
        
        objectiveTextBox.gameObject.SetActive(false);

        StopShrink(mapObjective);

    }

    private void Shrink(MapObjective mapObjective)
    {
        Debug.Log("START SHRINK");
        objectiveTimerRectTransform.gameObject.SetActive(true);
        
        // Use LeanTween to shrink the RectTransform's width
        LeanTween.size(objectiveTimerRectTransform, new Vector2(_targetWidth, objectiveTimerRectTransform.sizeDelta.y), mapObjective.GetExitTimeRemaining())
            .setEase(LeanTweenType.easeInOutSine); // Use an easing type for a smoother effect;
    }

    private void StopShrink(MapObjective mapObjective)
    {
        Debug.Log("STOP SHRINK");
        // Stop tween
        LeanTween.cancel(objectiveTimerRectTransform);
        
        // Reset rect transform's size
        objectiveTimerRectTransform.sizeDelta = new Vector2(_originalWidth, objectiveTimerRectTransform.sizeDelta.y);
        
        objectiveTimerRectTransform.gameObject.SetActive(false);
    }

    

    
}
