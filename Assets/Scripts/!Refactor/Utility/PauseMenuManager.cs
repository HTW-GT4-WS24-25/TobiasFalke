using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuManager : MonoBehaviour
{
    public UIDocument _pauseUI;
    private Button _continueButton;
    private Button _quitButton;
    

    private void OnEnable()
    {
        _quitButton = _pauseUI.rootVisualElement.Q("QuitButton") as Button;
        _continueButton = _pauseUI.rootVisualElement.Q("ContinueButton") as Button;
        _quitButton?.RegisterCallback<ClickEvent>(OnClickExitButton);
        _continueButton?.RegisterCallback<ClickEvent>(OnClickContinueButton);
        EventManagerR.AddListener<GameEvents.TogglePauseMenuEvent>(OnTogglePauseMenu);
    }

    private void OnDisable()
    {
        _continueButton?.UnregisterCallback<ClickEvent>(OnClickContinueButton);
        _quitButton?.UnregisterCallback<ClickEvent>(OnClickExitButton);
        EventManagerR.RemoveListener<GameEvents.TogglePauseMenuEvent>(OnTogglePauseMenu);
    }

    private void OnClickContinueButton(ClickEvent evt)
    {
        GameControllerR.Instance.ResumeGame(); 
    }

    private void OnClickExitButton(ClickEvent evt)
    {
        Application.Quit();
    }

    private void OnTogglePauseMenu(GameEvents.TogglePauseMenuEvent evt)
    {
        _pauseUI.rootVisualElement.style.display = evt.IsPaused ? DisplayStyle.Flex : DisplayStyle.None;
    }
}