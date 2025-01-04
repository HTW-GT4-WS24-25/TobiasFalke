using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _playButton;
    private Button _settingsButton;
    private Button _quitButton;

    private void Awake()
    {
        
        
        _uiDocument = GetComponent<UIDocument>();
        _playButton = _uiDocument.rootVisualElement.Q("PlayButton") as Button;
        _settingsButton = _uiDocument.rootVisualElement.Q("SettingsButton") as Button;
        _quitButton = _uiDocument.rootVisualElement.Q("QuitButton") as Button;

        _playButton?.RegisterCallback<ClickEvent>(OnClickPlayButton);
        _settingsButton?.RegisterCallback<ClickEvent>(OnClickSettingsButton);
        _quitButton?.RegisterCallback<ClickEvent>(OnClickExitButton);
    }

    private void Start()
    {
        AudioManager.Instance.PlayTrack("mainSceneMusic");
    }

    private void OnClickPlayButton(ClickEvent evt)
    {
        AudioManager.Instance.PlaySound("clickPlay");
        SceneLoader.Instance.LoadScene(SceneLoader.gameLevel);
    }
    private void OnClickExitButton(ClickEvent evt)
    {
        AudioManager.Instance.PlaySound("quitGame");
        //TODO Fix structure (currently also in GameManager)
        HideMenu();
        Application.Quit();
    }
    private void OnClickSettingsButton(ClickEvent evt)
    {
        AudioManager.Instance.PlaySound("openSettings");
        // SceneLoader.Instance.LoadSettingsMenu();
    }
    
    private void OnDisable()
    {
        _playButton.UnregisterCallback<ClickEvent>(OnClickPlayButton);
        _settingsButton.UnregisterCallback<ClickEvent>(OnClickSettingsButton);
        _quitButton.UnregisterCallback<ClickEvent>(OnClickExitButton);
    }

    private void HideMenu()
    {
        gameObject.SetActive(false);
    }
}
