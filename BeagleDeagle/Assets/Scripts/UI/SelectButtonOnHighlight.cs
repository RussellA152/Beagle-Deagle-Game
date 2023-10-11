using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

///-///////////////////////////////////////////////////////////
/// When a button is highlighted from a mouse, set it as the current selected button. This will prevent a button from being highlighted
/// and another button being selected at the same time.
public class SelectButtonOnHighlight : MonoBehaviour, IPointerEnterHandler
{
    // Implement the IPointerEnterHandler interface
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);

    }
}
