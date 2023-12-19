using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

///-///////////////////////////////////////////////////////////
/// When a button is highlighted from a mouse, set it as the current selected button. This will prevent a button from being highlighted
/// and another button being selected at the same time.
public class SelectButtonOnHighlight : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    private Button _button;
    public event Action<Button> onButtonSelected;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    // Implement the IPointerEnterHandler interface
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);

        // Tell listeners that this button has been selected
        onButtonSelected?.Invoke(_button);
        
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        // Tell listeners that this button has been selected
        onButtonSelected?.Invoke(_button);
    }


}
