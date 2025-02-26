using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using TMPro;

namespace Utility
{
    public class AuthManager : MonoBehaviour
    {
        private async void Start()
        {
            await UnityServices.InitializeAsync();
            await SignInAnonymous();
        }

        async Task SignInAnonymous()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sign in success!");
                Debug.Log("Player ID:" + AuthenticationService.Instance.PlayerId);
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}