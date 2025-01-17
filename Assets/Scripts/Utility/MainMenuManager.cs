using UnityEngine;
using UnityEngine.UIElements;
using Model;

namespace Utility
{
    public class MainMenuManager : MonoBehaviour
    {
        private UIDocument menuDocument;
        public UIDocument tutorialDocument;

        private Button playButton;
        private Button tutorialButton;
        private Button quitButton;
        private Button returnButton;

        private void Awake()
        {
            menuDocument = GetComponent<UIDocument>();
            playButton = menuDocument.rootVisualElement.Q<Button>("PlayButton");
            tutorialButton = menuDocument.rootVisualElement.Q<Button>("TutorialButton");
            quitButton = menuDocument.rootVisualElement.Q<Button>("QuitButton");
            returnButton = tutorialDocument.rootVisualElement.Q<Button>("ReturnButton");
            RegisterButtonCallbacks();
        }

        private void Start()
        {
        }

        private void RegisterButtonCallbacks()
        {
            playButton?.RegisterCallback<ClickEvent>(OnClickPlayButton);
            tutorialButton?.RegisterCallback<ClickEvent>(OnClickTutorialButton);
            quitButton?.RegisterCallback<ClickEvent>(OnClickExitButton);
            returnButton?.RegisterCallback<ClickEvent>(OnClickReturnButton);
        }

        private void OnClickPlayButton(ClickEvent evt)
        {
            AudioManager.Instance.PlaySound("clickPlay");
            SceneLoader.Instance.LoadScene(SceneLoader.gameLevel);
            EventManager.Broadcast(new GameModel.GameStateChanged(GameModel.GameState.Running));
        }

        private void OnClickExitButton(ClickEvent evt)
        {
            AudioManager.Instance.PlaySound("quitGame");
            HideMenu();
            Application.Quit();
        }

        private void OnClickTutorialButton(ClickEvent evt)
        {
            AudioManager.Instance.PlaySound("openTutorial");
            ShowTutorial();
        }

        private void OnClickReturnButton(ClickEvent evt)
        {
            AudioManager.Instance.PlaySound("closeTutorial");
            HideTutorial();
        }

        private void ShowTutorial()
        {
            tutorialDocument.sortingOrder = 1;
        }

        private void HideTutorial()
        {
            tutorialDocument.sortingOrder = -1;
        }

        private void OnDestroy()
        {
            UnregisterButtonCallbacks();
        }

        private void UnregisterButtonCallbacks()
        {
            playButton?.UnregisterCallback<ClickEvent>(OnClickPlayButton);
            tutorialButton?.UnregisterCallback<ClickEvent>(OnClickTutorialButton);
            quitButton?.UnregisterCallback<ClickEvent>(OnClickExitButton);
            returnButton?.UnregisterCallback<ClickEvent>(OnClickReturnButton);
        }

        private void HideMenu()
        {
            gameObject.SetActive(false);
        }
    }
}