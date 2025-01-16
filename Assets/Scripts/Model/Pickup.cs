using UnityEngine;
using Events;
using Interfaces;
using Utility;

namespace Model
{
    public enum PickupType
    {
        HealthBoost,
        HealthBoom,
        SpecialBoost,
        SpecialBoom,
        SpeedBoost,
        SpeedBoom,
        JumpBoost,
        JumpBoom,
        ScoreBoost,
        ScoreBoom,
        ScoreMultiplierBoost
    }

    public class Pickup : FallingObject
    {
        [SerializeField] private PickupType pickupType;
   
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            EventManager.Broadcast(new PlayerEvent.PickupCollision(pickupType));
            Destroy(gameObject);
        }
    }
}