using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _playButton;
    private Button _tutorialButton;
    private Button _quitButton;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _playButton = _uiDocument.rootVisualElement.Q("PlayButton") as Button;
        _tutorialButton = _uiDocument.rootVisualElement.Q("TutorialButton") as Button;
        _quitButton = _uiDocument.rootVisualElement.Q("QuitButton") as Button;

        _playButton?.RegisterCallback<ClickEvent>(OnClickPlayButton);
        _tutorialButton?.RegisterCallback<ClickEvent>(OnClickTutorialButton);
        _quitButton?.RegisterCallback<ClickEvent>(OnClickExitButton);
    }

    private void Start()
    {
        AudioManager.Instance.PlayTrack("mainMenuMusic");
    }

    private void OnClickPlayButton(ClickEvent evt)
    {
        AudioManager.Instance.PlaySound("clickPlay");
        SceneLoader.Instance.LoadScene(SceneLoader.GameLevelScene);
    }
    private void OnClickExitButton(ClickEvent evt)
    {
        AudioManager.Instance.PlaySound("quitGame");
        //TODO Fix structure (currently also in GameManager)
        HideMenu();
        Application.Quit();
    }
    private void OnClickTutorialButton(ClickEvent evt)
    {
        AudioManager.Instance.PlaySound("openSettings");
        SceneLoader.Instance.LoadScene(SceneLoader.TutorialScene);
    }
    
    private void OnDisable()
    {
        _playButton.UnregisterCallback<ClickEvent>(OnClickPlayButton);
        _tutorialButton.UnregisterCallback<ClickEvent>(OnClickTutorialButton);
        _quitButton.UnregisterCallback<ClickEvent>(OnClickExitButton);
    }

    private void HideMenu()
    {
        gameObject.SetActive(false);
    }
}
