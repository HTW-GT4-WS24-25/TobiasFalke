using System;
using Model;
using UnityEngine;
using UnityEngine.UIElements;
using PlayFab;
using PlayFab.ClientModels;
using Button = UnityEngine.UIElements.Button;

namespace Menus
{
    public class LoginMenuManager : MonoBehaviour
    {
        private static Color DARK_GREEN = new Color(0f, 0.5f, 0f, 1f);
        private static Color DARK_RED = new Color(0.6f, 0f, 0f, 1f);
        
        private UIDocument uiDocument;
        
        private Button loginButton;
        private Button registerButton;
        private Button resetPasswordButton;
        private Button exitButton;
        
        private TextField emailInputField;
        private TextField passwordInputField;

        private Label notificationLabel;
        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
            loginButton = uiDocument.rootVisualElement.Q<Button>("LoginButton");
            registerButton = uiDocument.rootVisualElement.Q<Button>("RegisterButton");
            resetPasswordButton = uiDocument.rootVisualElement.Q<Button>("ResetPasswordButton");
            exitButton = uiDocument.rootVisualElement.Q<Button>("ExitButton");

            emailInputField = uiDocument.rootVisualElement.Q<TextField>("EmailInput");
            passwordInputField = uiDocument.rootVisualElement.Q<TextField>("PasswordInput");
            notificationLabel = uiDocument.rootVisualElement.Q<Label>("Notification");
            RegisterButtonCallbacks();
        }
        private void RegisterButtonCallbacks()
        {
            loginButton?.RegisterCallback<ClickEvent>(OnClickLoginButton);
            registerButton?.RegisterCallback<ClickEvent>(OnClickRegisterButton);
            resetPasswordButton?.RegisterCallback<ClickEvent>(OnClickResetPasswordButton);
            exitButton?.RegisterCallback<ClickEvent>(OnClickExitButton);
        }

        private void OnClickExitButton(ClickEvent evt)
        {
            throw new NotImplementedException();
        }

        private void OnClickResetPasswordButton(ClickEvent evt)
        {
            var requst = new SendAccountRecoveryEmailRequest
            {
                Email = emailInputField.text,
                TitleId = "DF557"
            };
            PlayFabClientAPI.SendAccountRecoveryEmail(requst, OnPasswordReset, OnError);
        }

        private void OnPasswordReset(SendAccountRecoveryEmailResult obj)
        {
            notificationLabel.style.color = DARK_RED;
            notificationLabel.text = "Password rest mail has been sent.";
        }

        private void OnClickRegisterButton(ClickEvent evt)
        {
            if (passwordInputField.text.Length < 6)
            {
                notificationLabel.style.color = DARK_RED;
                notificationLabel.text = "Password is too short! Needs at least 6 characters.";
                return;
            }
            var request = new RegisterPlayFabUserRequest {
                Email = emailInputField.text,
                Password = passwordInputField.text,
                RequireBothUsernameAndEmail = false
            };
            
            PlayFabClientAPI.RegisterPlayFabUser(request,OnRegisterSuccess, OnError);
        }

        private void OnError(PlayFabError error)
        {
            notificationLabel.style.color = DARK_RED;
            notificationLabel.text = error.ErrorMessage;
            
            Debug.Log(error.GenerateErrorReport());
        }

        private void OnRegisterSuccess(RegisterPlayFabUserResult obj)
        {
            notificationLabel.style.color = DARK_GREEN;
            notificationLabel.text = "Success!";
            //TODO: close login menu 
        }

        private void OnClickLoginButton(ClickEvent evt)
        {
            var request = new LoginWithEmailAddressRequest
            {
                Email = emailInputField.text,
                Password = passwordInputField.text,
            };
            PlayFabClientAPI.LoginWithEmailAddress(request,OnLoginSuccess, OnError);

        }

        private void OnLoginSuccess(LoginResult obj)
        {
            notificationLabel.style.color = DARK_GREEN;
            notificationLabel.text = "Successfully logged in!";
        }

        private void OnDestroy()
        {
            UnregisterButtonCallbacks();
        }

        private void UnregisterButtonCallbacks()
        {
            loginButton?.UnregisterCallback<ClickEvent>(OnClickLoginButton);
            registerButton?.UnregisterCallback<ClickEvent>(OnClickRegisterButton);
            resetPasswordButton?.UnregisterCallback<ClickEvent>(OnClickResetPasswordButton);
            exitButton?.UnregisterCallback<ClickEvent>(OnClickExitButton);
        }

    }
}
