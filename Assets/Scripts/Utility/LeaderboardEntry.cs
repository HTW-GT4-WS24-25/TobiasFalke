using UnityEngine;

namespace Utility
{
    public class LeaderboardEntry
    {
        public LeaderboardEntry(int position, string playFabId, int statValue)
        {
            Position = position;
            PlayFabId = playFabId;
            StatValue = statValue;
        }

        public int Position { get; set; }
        public string PlayFabId { get; set; }
        public int StatValue { get; set; }
    }
}