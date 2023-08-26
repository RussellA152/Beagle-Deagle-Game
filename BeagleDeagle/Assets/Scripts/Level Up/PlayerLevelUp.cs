using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-///////////////////////////////////////////////////////////
/// When performing certain actions such as killing enemies, the player will automatically gain experience
/// which this class will store and notify other scripts about.
/// 
public class PlayerLevelUp : MonoBehaviour
{
    [SerializeField] private PlayerEvents playerEvents;
    
    // The current rank that the player is at
    private int _currentLevel;
    
    
}
