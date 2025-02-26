using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.RemoteConfig;

namespace Utility
{
    public class GameConfig : MonoBehaviour
    {
        public static GameConfig Instance { get; set; }

        public int MaxHealthPoints;
        public int MaxSpecialPoints;
        public float BaseSpeed;
        public float MaxSpeedMultiplier;
        public float JumpHeight;
        public float JumpDuration;
        public float GrindActionScore;
        public float TrickActionScore;
        public float TrickActionDuration;
        public float SpecialActionDuration;
        public float InvincibilityDuration;
        public int StartingStage;
        public float StageDuration;
        public float SpeedIncreasePerStage;
        public float BaseStageSpeed;
        public float BaseStageWidth;
        public float BaseStageHeight;
        public float BaseObstacleSpawnInterval;
        public float BasePickupSpawnInterval;

        public struct userAttributes
        {
            public int score;
        }

        public struct appAttributes
        {

        }

        async Task InitializeRemoteConfigAsync()
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        private async void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
            
            await InitializeRemoteConfigAsync();

            var uaStruct = new userAttributes
            {
                score = 0
            };

            RemoteConfigService.Instance.FetchConfigs(uaStruct, new appAttributes());
            RemoteConfigService.Instance.FetchCompleted += RemoteConfigLoaded;
            RemoteConfigService.Instance.SetEnvironmentID("ca19571a-8000-4cee-b0f2-f9832659a0b8");
            await InitializeGameConfiguration();
        }
        
        private async Task InitializeGameConfiguration()
        {
            BaseObstacleSpawnInterval = await SetConfigValue(false, "BaseObstacleSpawnInterval");
            BaseSpeed = await SetConfigValue(false, "BaseSpeed");
            BaseStageHeight = await SetConfigValue(false, "BaseStageHeight");
            BaseStageSpeed = await SetConfigValue(false, "BaseStageSpeed");
            BaseStageWidth = await SetConfigValue(false, "BaseStageWidth");
            GrindActionScore = await SetConfigValue(false, "GrindActionScore");
            InvincibilityDuration = await SetConfigValue(false, "InvincibilityDuration");
            JumpDuration = await SetConfigValue(false, "JumpDuration");
            JumpHeight = await SetConfigValue(false, "JumpHeight");
            MaxHealthPoints = (int)await SetConfigValue(true, "MaxHealthPoints");
            MaxSpecialPoints = (int)await SetConfigValue(true, "MaxSpecialPoints");
            MaxSpeedMultiplier = await SetConfigValue(false, "MaxSpeedMultiplier");
            SpecialActionDuration = await SetConfigValue(false, "SpecialActionDuration");
            SpeedIncreasePerStage = await SetConfigValue(false, "SpeedIncreasePerStage");
            StageDuration = await SetConfigValue(false, "StageDuration");
            StartingStage = (int)await SetConfigValue(true, "StartingStage");
            TrickActionDuration = await SetConfigValue(false, "TrickActionDuration");
            TrickActionScore = await SetConfigValue(false, "TrickActionScore");
        }

        private async Task<float> SetConfigValue(bool isInteger, string key)
        {
            return isInteger switch
            {
                true => RemoteConfigService.Instance.appConfig.GetFloat(key),
                false => RemoteConfigService.Instance.appConfig.GetInt(key)
            };
        }

        private void RemoteConfigLoaded(ConfigResponse configResponse)
        {
            switch (configResponse.requestOrigin)
            {
                case ConfigOrigin.Default:
                    Debug.Log("default settings");
                    break;
                case ConfigOrigin.Cached:
                    Debug.Log("cached settings");
                    break;
                case ConfigOrigin.Remote:
                    Debug.Log("remote settings");
                    break;
            }
        }
    }
}