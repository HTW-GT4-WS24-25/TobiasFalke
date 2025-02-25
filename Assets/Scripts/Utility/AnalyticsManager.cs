using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

namespace Utility
{
    public class AnalyticsManager : MonoBehaviour
    {
        private static AnalyticsManager Instance;
        private bool isInitialized;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private async void Start()
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            isInitialized = true;
        }

        public void RegisterPlayerData(string variable, int value)
        {
            if (!isInitialized)
            {
                return;
            }

            CustomEvent myEvent = new CustomEvent("recordEvent")
            {
                { variable, value }
            };
            AnalyticsService.Instance.RecordEvent(myEvent);
        }
    }
}