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

    private void OnEnable()
    {
        buttonComponent.onClick.AddListener(ChooseLevel);
    }

    private void OnDisable()
    {
        buttonComponent.onClick.RemoveListener(ChooseLevel);
    }

    private void ChooseLevel()
    {
        SceneLoader.Instance.InvokeLevelChosen(selectableLevel);
    }

}
