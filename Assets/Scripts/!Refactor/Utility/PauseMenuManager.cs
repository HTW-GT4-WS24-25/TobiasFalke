using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuManager : MonoBehaviour
{
    public UIDocument pauseUI;
    private Button continueButton;
    private Button quitButton;

    private void Awake()
    {
        pauseUI.rootVisualElement.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        quitButton = pauseUI.rootVisualElement.Q<Button>("QuitButton");
        continueButton = pauseUI.rootVisualElement.Q<Button>("ContinueButton");

        quitButton?.RegisterCallback<ClickEvent>(OnClickExitButton);
        continueButton?.RegisterCallback<ClickEvent>(OnClickContinueButton);
        EventManagerR.AddListener<GameEvents.TogglePauseMenuEvent>(OnTogglePauseMenu);
    }

    private void OnDisable()
    {
        continueButton?.UnregisterCallback<ClickEvent>(OnClickContinueButton);
        quitButton?.UnregisterCallback<ClickEvent>(OnClickExitButton);
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
        pauseUI.rootVisualElement.style.display = evt.IsPaused ? DisplayStyle.Flex : DisplayStyle.None;
        Debug.Log("Pause Menu " + (evt.IsPaused ? "Displayed" : "Hidden"));
    }
}