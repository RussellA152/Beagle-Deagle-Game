using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUISounds", menuName = "ScriptableObjects/UI/Sounds")]
public class UISounds : ScriptableObject
{
    // Sound to play when a UI button has been hovered over with a mouse cursor
    public AudioClip selectSound;
    
    // Sound to play when clicking on the selected button
    public AudioClip clickSound;

    [Range(0.1f,1f)]
    public float volume;
}
