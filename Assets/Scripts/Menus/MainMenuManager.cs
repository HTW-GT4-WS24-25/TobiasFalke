using Events;
using Model;
using UnityEditor;
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
        public UIDocument loginDocument;

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
            SubscribeEvents();

            if (UserAccountManager.Instance.IsUserLoggedIn())
            {
                UserAccountManager.Instance.GetProfileImageID(
                    SetProfilePictureButton,
                    Debug.Log
                );
            }
            else
            {
                StyleBackground profileImage = new StyleBackground( AssetDatabase.LoadAssetAtPath<Sprite>(Sprites.UnkownProfilePath));
                profileButton.style.backgroundImage = profileImage;
            };
        }

        private void OnProfileImageChanged(UserEvent.ProfileImageChanged obj)
        {
            SetProfilePictureButton(obj.ProfileImageID);
        }

        private void SetProfilePictureButton(int index)
        {
                string fullImagePath = Sprites.ProfileImagesPath + Sprites.ProfileImageFileBase + index + ".png";
                StyleBackground profileImage = new StyleBackground( AssetDatabase.LoadAssetAtPath<Sprite>(fullImagePath));
                profileButton.style.backgroundImage = profileImage;
        }

        private void OnUserSignedIn(UserEvent.UserSignedIn obj)
        {
            UserAccountManager.Instance.GetProfileImageID(SetProfilePictureButton,error =>{Debug.Log(error);});
        }
        

        private void RegisterButtonCallbacks()
        {
            playButton?.RegisterCallback<ClickEvent>(OnClickPlayButton);
            tutorialButton?.RegisterCallback<ClickEvent>(OnClickTutorialButton);
            quitButton?.RegisterCallback<ClickEvent>(OnClickExitButton);
            returnButton?.RegisterCallback<ClickEvent>(OnClickReturnButton);
            profileButton?.RegisterCallback<ClickEvent>(OnClickProfileButton);
        }

        private void SubscribeEvents()
        {
            EventManager.Add<UserEvent.UserSignedIn>(OnUserSignedIn);
            EventManager.Add<UserEvent.ProfileImageChanged>(OnProfileImageChanged);
        }

        private void OnClickProfileButton(ClickEvent evt)
        {
            ShowLogin();
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
        
        private void ShowLogin()
        {
            loginDocument.sortingOrder = 1;
        }

        private void OnDestroy()
        {
            UnregisterButtonCallbacks();
            UnsubscribeEvents();
        }

        private void UnregisterButtonCallbacks()
        {
            playButton?.UnregisterCallback<ClickEvent>(OnClickPlayButton);
            tutorialButton?.UnregisterCallback<ClickEvent>(OnClickTutorialButton);
            quitButton?.UnregisterCallback<ClickEvent>(OnClickExitButton);
            returnButton?.UnregisterCallback<ClickEvent>(OnClickReturnButton);
            profileButton?.UnregisterCallback<ClickEvent>(OnClickProfileButton);
        }

        private void UnsubscribeEvents()
        {
            EventManager.Remove<UserEvent.UserSignedIn>(OnUserSignedIn);
            EventManager.Remove<UserEvent.ProfileImageChanged>(OnProfileImageChanged);
        }

        private void HideMenu()
        {
            gameObject.SetActive(false);
        }
    }
}