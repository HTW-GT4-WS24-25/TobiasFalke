using Model;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;
using static Utility.GameConstants;

namespace Menus
{
    public class MainMenuManager : MonoBehaviour
    {
        private UIDocument menuDocument;
        public UIDocument tutorialDocument;

        private Button playButton;
        private Button tutorialButton;
        private Button quitButton;
        private Button returnButton;
        private Button profileButton;

        private void Awake()
        {
            menuDocument = GetComponent<UIDocument>();
            playButton = menuDocument.rootVisualElement.Q<Button>("PlayButton");
            tutorialButton = menuDocument.rootVisualElement.Q<Button>("TutorialButton");
            quitButton = menuDocument.rootVisualElement.Q<Button>("QuitButton");
            returnButton = tutorialDocument.rootVisualElement.Q<Button>("ReturnButton");
            profileButton = menuDocument.rootVisualElement.Q<Button>("ProfileButton");
            RegisterButtonCallbacks();
        }

        private void RegisterButtonCallbacks()
        {
            playButton?.RegisterCallback<ClickEvent>(OnClickPlayButton);
            tutorialButton?.RegisterCallback<ClickEvent>(OnClickTutorialButton);
            quitButton?.RegisterCallback<ClickEvent>(OnClickExitButton);
            returnButton?.RegisterCallback<ClickEvent>(OnClickReturnButton);
            profileButton?.RegisterCallback<ClickEvent>(OnClickProfileButton);
        }

        private void OnClickProfileButton(ClickEvent evt)
        {
            //TODO: open Login Menu onClick 
            throw new System.NotImplementedException();
            
        }

        private static void OnClickPlayButton(ClickEvent evt)
        {
            AudioManager.Instance.PlaySound(Audio.MenuClickSFX);
            EventManager.Trigger(new GameModel.GameStateChanged(GameModel.GameState.Running));
            SceneLoader.Instance.LoadScene(Scenes.Level);
        }

        private void OnClickExitButton(ClickEvent evt)
        {
            AudioManager.Instance.PlaySound(Audio.MenuClickSFX);
            HideMenu();
            Application.Quit();
        }

        private void OnClickTutorialButton(ClickEvent evt)
        {
            AudioManager.Instance.PlaySound(Audio.MenuClickSFX);
            ShowTutorial();
        }

        private void OnClickReturnButton(ClickEvent evt)
        {
            AudioManager.Instance.PlaySound(Audio.MenuClickSFX);
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
            profileButton?.UnregisterCallback<ClickEvent>(OnClickProfileButton);
        }

        private void HideMenu()
        {
            gameObject.SetActive(false);
        }
    }
}