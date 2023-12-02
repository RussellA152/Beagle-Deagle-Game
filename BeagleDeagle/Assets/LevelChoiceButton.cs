using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelChoiceButton : MonoBehaviour
{
    [HideInInspector]
    public Button buttonComponent;
    
    [Range(0,20)]
    public int selectableLevel;

    private void Awake()
    {
        buttonComponent = GetComponent<Button>();
    }
    
    private void Start()
    {
        StartMenuManager.Instance.onPlayerChoseCharacter += DisableOnLevelChoice;
    }

    private void OnEnable()
    {
        buttonComponent.onClick.AddListener(ChooseLevel);
    }

    private void OnDisable()
    {
        buttonComponent.onClick.RemoveListener(ChooseLevel);
        StartMenuManager.Instance.onPlayerChoseCharacter -= DisableOnLevelChoice;
    }

    private void ChooseLevel()
    {
        StartMenuManager.Instance.InvokeLevelChosen(selectableLevel);
    }

    private void DisableOnLevelChoice()
    {
        buttonComponent.enabled = false;
    }
}
