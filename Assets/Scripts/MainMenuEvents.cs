using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _playButton;
    private Button _settingsButton;
    private Button _exitButton;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _playButton = _uiDocument.rootVisualElement.Q("PlayButton") as Button;
        _settingsButton = _uiDocument.rootVisualElement.Q("SettingsButton") as Button;
        _exitButton = _uiDocument.rootVisualElement.Q("ExitButton") as Button;
        
        _playButton.RegisterCallback<ClickEvent>(OnClickPlayButton);
        _settingsButton.RegisterCallback<ClickEvent>(OnClickSettingsButton);
        _exitButton.RegisterCallback<ClickEvent>(OnClickExitButton);
        
    }

    private void OnClickPlayButton(ClickEvent evt)
    {
        throw new NotImplementedException();
    }
    private void OnClickExitButton(ClickEvent evt)
    {
        throw new NotImplementedException();
    }
    private void OnClickSettingsButton(ClickEvent evt)
    {
        throw new NotImplementedException();
    }


    private void OnDisable()
    {
        _playButton.UnregisterCallback<ClickEvent>(OnClickPlayButton);
        _settingsButton.UnregisterCallback<ClickEvent>(OnClickSettingsButton);
        _exitButton.UnregisterCallback<ClickEvent>(OnClickExitButton);
    }
}
