using System;
using System.IO;
using System.Linq;
using Events;
using Model;
using UnityEngine;
using UnityEngine.UIElements;
using PlayFab;
using PlayFab.ClientModels;
using UnityEditor;
using Utility;
using Button = UnityEngine.UIElements.Button;
using Event = UnityEngine.Event;

namespace Menus
{
    public class LoginMenuManager : MonoBehaviour
    {
        private static Color DARK_GREEN = new Color(0f, 0.5f, 0f, 1f);
        private static Color DARK_RED = new Color(0.6f, 0f, 0f, 1f);
        
        private Sprite[] profileSpites;
        private int currentImageIndex = 0;
        
        private UIDocument uiDocument;
        
        private Button loginButton;
        private Button registerButton;
        private Button resetPasswordButton;
        private Button exitButton;
        private Button selectLeftImageButton;
        private Button selectRightImageButton;
        private Button safeImageChoice;
        private VisualElement selectionImage;
        
        private TextField emailInputField;
        private TextField passwordInputField;

        private Label notificationLabel;
        private Label profileImageNotification;
        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
            loginButton = uiDocument.rootVisualElement.Q<Button>("LoginButton");
            registerButton = uiDocument.rootVisualElement.Q<Button>("RegisterButton");
            resetPasswordButton = uiDocument.rootVisualElement.Q<Button>("ResetPasswordButton");
            exitButton = uiDocument.rootVisualElement.Q<Button>("ExitButton");
            
            selectLeftImageButton = uiDocument.rootVisualElement.Q<Button>("SelectLeftImageButton");
            selectRightImageButton = uiDocument.rootVisualElement.Q<Button>("SelectRightImageButton");
            safeImageChoice = uiDocument.rootVisualElement.Q<Button>("SaveImageButton");
            selectionImage = uiDocument.rootVisualElement.Q<VisualElement>("SelectionImage");
            profileImageNotification = uiDocument.rootVisualElement.Q<Label>("ProfileImageNotification");

            emailInputField = uiDocument.rootVisualElement.Q<TextField>("EmailInput");
            passwordInputField = uiDocument.rootVisualElement.Q<TextField>("PasswordInput");
            notificationLabel = uiDocument.rootVisualElement.Q<Label>("Notification");
            RegisterButtonCallbacks();
            SubscribeEvents();
        }

        private void Start()
        {
            profileSpites = Resources.LoadAll<Sprite>(GameConstants.Sprites.ProfileImagesPath);
            Debug.Log("Number of Sprites:" + profileSpites.Length + " First Sprite:" + profileSpites[0].name);
            /*imagePaths = Directory.GetFiles(GameConstants.Sprites.ProfileImagesPath, GameConstants.Sprites.ProfileImageFileBase + "*.png")
                .OrderBy(path => int.Parse(Path.GetFileNameWithoutExtension(path).Replace(GameConstants.Sprites.ProfileImageFileBase, "")))
                .ToArray();*/
        }

        private void RegisterButtonCallbacks()
        {
            exitButton?.RegisterCallback<ClickEvent>(OnClickExitButton);

            loginButton?.RegisterCallback<ClickEvent>(OnClickLoginButton);
            registerButton?.RegisterCallback<ClickEvent>(OnClickRegisterButton);
            resetPasswordButton?.RegisterCallback<ClickEvent>(OnClickResetPasswordButton);
            
            selectLeftImageButton?.RegisterCallback<ClickEvent>(OnClickSelectLeftImageButton);
            selectRightImageButton?.RegisterCallback<ClickEvent>(OnClickSelectRightImageButton);
            safeImageChoice?.RegisterCallback<ClickEvent>(OnClickSafeImageChoice);

        }
        
        private void SubscribeEvents()
        {
            EventManager.Add<UserEvent.LoginError>(OnError);
            EventManager.Add<UserEvent.AccountActionSuccess>(OnAccountActionSuccess);
            EventManager.Add<UserEvent.ProfileImageError>(OnProfileImageError);
            EventManager.Add<UserEvent.ProfileImageChanged>(OnProfileImageChanged);
        }

        private void OnProfileImageChanged(UserEvent.ProfileImageChanged obj)
        {
            profileImageNotification.style.color = DARK_GREEN;
            profileImageNotification.text = "Sucess!";
        }

        private void OnProfileImageError(UserEvent.ProfileImageError obj)
        {
            var errorMessage = obj.Message;
            profileImageNotification.style.color = DARK_RED;
            profileImageNotification.text = errorMessage;
            
        }

        private void OnClickSafeImageChoice(ClickEvent evt)
        {
            if (UserAccountManager.Instance.IsUserLoggedIn())
            {
                Sprite profileSprite = profileSpites[currentImageIndex];
                int imageNumber = int.Parse(profileSprite.name.Replace(GameConstants.Sprites.ProfileImageFileBase, ""));

                UserAccountManager.Instance.SetProfileImageID(imageNumber);
                
            }
            
        }

        private void OnClickSelectRightImageButton(ClickEvent evt)
        {
            if (profileSpites is not { Length: > 0 }) return;
            currentImageIndex = (currentImageIndex + 1) % profileSpites.Length;
            UpdateProfileImage();
        }

        private void OnClickSelectLeftImageButton(ClickEvent evt)
        {
            if (profileSpites is not { Length: > 0 }) return;
            currentImageIndex = (currentImageIndex - 1 + profileSpites.Length) % profileSpites.Length;
            UpdateProfileImage();
        }

        private void UpdateProfileImage()
        {
            Sprite profileSprite = profileSpites[currentImageIndex];
            StyleBackground profileImage = new StyleBackground( profileSprite);
            selectionImage.style.backgroundImage = profileImage;
        }


        private void OnClickExitButton(ClickEvent evt)
        {
            HideLogin();
        }

        private void OnClickResetPasswordButton(ClickEvent evt)
        {
            UserAccountManager.Instance.ResetPassword(emailInputField.text);
        }

        private void OnClickRegisterButton(ClickEvent evt)
        {
            if (passwordInputField.text.Length < 6)
            {
                notificationLabel.style.color = DARK_RED;
                notificationLabel.text = "Password is too short! Needs at least 6 characters.";
                return;
            }
            
            UserAccountManager.Instance.Register(emailInputField.text,passwordInputField.text);
        }

        private void OnClickLoginButton(ClickEvent evt)
        {
            UserAccountManager.Instance.LogIn(emailInputField.text, passwordInputField.text);
        }

        private void OnError(UserEvent.LoginError evt)
        {
            var error = evt.PlayFabError;
            notificationLabel.style.color = DARK_RED;
            notificationLabel.text = error.ErrorMessage;
            
            Debug.Log(error.GenerateErrorReport());
        }

        private void OnAccountActionSuccess(UserEvent.AccountActionSuccess evt)
        {
            notificationLabel.style.color = DARK_GREEN;
            notificationLabel.text = evt.Message;
        }
        
        private void HideLogin()
        {
            uiDocument.sortingOrder = -1;
        }

        private void OnDestroy()
        {
            UnregisterButtonCallbacks();
            UnsubscribeEvents();
        }

        private void UnregisterButtonCallbacks()
        {
            exitButton?.UnregisterCallback<ClickEvent>(OnClickExitButton);
            
            loginButton?.UnregisterCallback<ClickEvent>(OnClickLoginButton);
            registerButton?.UnregisterCallback<ClickEvent>(OnClickRegisterButton);
            resetPasswordButton?.UnregisterCallback<ClickEvent>(OnClickResetPasswordButton);
            
            selectLeftImageButton?.UnregisterCallback<ClickEvent>(OnClickSelectLeftImageButton);
            selectRightImageButton?.UnregisterCallback<ClickEvent>(OnClickSelectRightImageButton);
            safeImageChoice?.RegisterCallback<ClickEvent>(OnClickSafeImageChoice);

        }
        private void UnsubscribeEvents()
        {
            EventManager.Remove<UserEvent.LoginError>(OnError);
            EventManager.Remove<UserEvent.AccountActionSuccess>(OnAccountActionSuccess);
        }

    }
}
