using Model;
using UnityEngine;
using PlayFab;
using Event = Utility.Event;

namespace Events
{
    public static class UserEvent
    {
        public class UserSignedIn : Event {}

        public class LoginError : Event
        {
            public PlayFabError PlayFabError { get; }

            public LoginError(PlayFabError error)
            {
                PlayFabError = error;
            }
        }

        public class AccountActionSuccess : Event
        {
            public string Message { get; }

            public AccountActionSuccess(string message)
            {
                Message = message;
            }
        }

        public class ProfileImageChanged : Event
        {
            public int ProfileImageID { get; }

            public ProfileImageChanged(int profileImageID)
            {
                ProfileImageID = profileImageID;
            }
        }

        public class ProfileImageError : Event
        {
            public string Message { get; }

            public ProfileImageError(string message)
            {
                Message = message;
            }
        }
            
    }
}