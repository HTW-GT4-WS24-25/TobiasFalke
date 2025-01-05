using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuEvents : MonoBehaviour
{
    private UIDocument _pauseUI;
    private Button _continueButton;
    private Button _quitButton;
    
    void Awake()
    {
        _pauseUI = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        _quitButton = _pauseUI.rootVisualElement.Q("QuitButton") as Button;
        _continueButton = _pauseUI.rootVisualElement.Q("ContinueButton") as Button;
        
        _quitButton?.RegisterCallback<ClickEvent>(OnClickExitButton);
        _continueButton?.RegisterCallback<ClickEvent>(OnClickContinueButton);
    }

    private void OnClickContinueButton(ClickEvent evt)
    {
        GameManager.Instance.Resume();
        Debug.Log("Game Continue Clicked");
    }
    // Update is called once per frame
    private void OnDisable()
    {
        _continueButton.UnregisterCallback<ClickEvent>(OnClickContinueButton);
        _quitButton.UnregisterCallback<ClickEvent>(OnClickExitButton);
    }

    private void OnClickExitButton(ClickEvent evt)
    {
        //TODO Manage QUIT Game somewhere central
        Application.Quit();
    }
}
