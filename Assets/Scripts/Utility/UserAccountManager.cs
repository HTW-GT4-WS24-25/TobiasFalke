using System;
using System.Collections.Generic;
using Events;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Utility
{
    public class UserAccountManager : MonoBehaviour
    {
        public static UserAccountManager Instance { get; private set; }
        public string CurrentPlayerPlayFabId;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            EventManager.Add<PlayerEvent.GameOverTriggered>(OnGameOverTriggered);
        }

        private void OnGameOverTriggered(PlayerEvent.GameOverTriggered obj)
        {
            if (IsUserLoggedIn()) SendLeaderboard((int)obj.ScoreOnLoose);
        }

        public bool IsUserLoggedIn()
        {
            return PlayFabClientAPI.IsClientLoggedIn();
        }

        public void SendLeaderboard(int score)
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate {
                        StatisticName = "HighScore",
                        Value = score
                    }
                }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, error => {Debug.LogError(error.GenerateErrorReport());} );
        }

        public void GetLeaderBoard(Action<List<LeaderboardEntry>> onSuccess, Action<string> onError)
        {
            var request = new GetLeaderboardRequest
            {
                StatisticName = "HighScore",
                StartPosition = 0,
                MaxResultsCount = 10
            };
            PlayFabClientAPI.GetLeaderboard(request,
                result =>
            {
                List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
                foreach (var item in result.Leaderboard)
                {
                    leaderboardEntries.Add(item.PlayFabId == CurrentPlayerPlayFabId
                        ? new LeaderboardEntry(item.Position, "You", item.StatValue)
                        : new LeaderboardEntry(item.Position, item.PlayFabId, item.StatValue));
                }
                
                onSuccess(leaderboardEntries);
            }, error =>
            {
                Debug.LogError(error.GenerateErrorReport());
                onError(error.GenerateErrorReport());
            } );
        }

        private void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult obj)
        {
            Debug.Log("Successfully send Leaderboard Data");
        }
        
        public void GetProfileImageID(Action<int> onSuccess, Action<string> onError)
        {
            var request = new GetUserDataRequest()
            {
                Keys = new List<string>() { "ProfileImageID" }
            };

            PlayFabClientAPI.GetUserData(request,
                success =>
                {
                    int profileImageID = int.Parse(success.Data["ProfileImageID"].Value);
                    onSuccess(profileImageID);
                },
                error =>
                {
                    Debug.Log(error.GenerateErrorReport());
                    onError(error.GenerateErrorReport());
                });
        }

        public void SetProfileImageID(int profileImageID)
        {
            var request = new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>
                {
                    { "ProfileImageID", profileImageID.ToString() }
                }
            };
            PlayFabClientAPI.UpdateUserData(request, success =>{ EventManager.Trigger(new UserEvent.ProfileImageChanged(profileImageID)); }, OnProfileImageError);
            
        }

        private void OnProfileImageError(PlayFabError obj)
        {
            EventManager.Trigger(new UserEvent.ProfileImageError("Failed To sent Data"));
        }


        public void LogOut()
        {
            throw new NotImplementedException();
        }

        public void LogIn(string email, string password)
        {
            var request = new LoginWithEmailAddressRequest
            {
                Email = email,
                Password = password,
            };
            PlayFabClientAPI.LoginWithEmailAddress(request,OnLoginSuccess, OnError);
        }

        private void OnLoginSuccess(LoginResult obj)
        {
            Debug.Log(obj);
            CurrentPlayerPlayFabId = obj.PlayFabId;
            EventManager.Trigger(new UserEvent.AccountActionSuccess("successfully logged in!"));
            EventManager.Trigger(new UserEvent.UserSignedIn());
        }

        public void Register(string email, string password)
        {
            var request = new RegisterPlayFabUserRequest {
                Email = email,
                Password = password,
                RequireBothUsernameAndEmail = false
            };
            
            PlayFabClientAPI.RegisterPlayFabUser(request,OnRegisterSuccess, OnError);
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
                {
                    Data = new Dictionary<string, string> { { "ProfileImageID", "1" } }
                },
                result => Debug.Log("Successfully Profile Image sent Data"),
                error => Debug.Log(error.GenerateErrorReport())
                );
        }

        private void OnRegisterSuccess(RegisterPlayFabUserResult obj)
        {
            
            EventManager.Trigger(new UserEvent.AccountActionSuccess("Successfully Registered!"));
            EventManager.Trigger(new UserEvent.UserSignedIn());
        }

        public void ResetPassword(string email)
        {
            var requst = new SendAccountRecoveryEmailRequest
            {
                Email = email,
                TitleId = "DF557"
            };
            PlayFabClientAPI.SendAccountRecoveryEmail(requst, OnPasswordReset, OnError);
        }

        private void OnPasswordReset(SendAccountRecoveryEmailResult obj)
        {
            EventManager.Trigger(new UserEvent.AccountActionSuccess("Password rest mail has been sent."));
        }

        private void OnError(PlayFabError obj)
        {
            EventManager.Trigger(new UserEvent.LoginError(obj));
        }
    }
}