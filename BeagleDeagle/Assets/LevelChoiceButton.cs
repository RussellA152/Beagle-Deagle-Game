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
        SceneLoader.Instance.onLevelChosen += DisableOnLevelChoice;
    }

    private void OnEnable()
    {
        buttonComponent.onClick.AddListener(ChooseLevel);
    }

    private void OnDisable()
    {
        buttonComponent.onClick.RemoveListener(ChooseLevel);
        SceneLoader.Instance.onLevelChosen -= DisableOnLevelChoice;
    }

    private void ChooseLevel()
    {
        SceneLoader.Instance.InvokeLevelChosen(selectableLevel);
    }

    private void DisableOnLevelChoice()
    {
        buttonComponent.enabled = false;
    }
}
