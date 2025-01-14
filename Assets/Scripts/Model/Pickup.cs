using Events;
using UnityEngine;
using UnityEngine.Serialization;

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

    public class Pickup : MonoBehaviour, IObject
    {
        [FormerlySerializedAs("itemType")] [SerializeField]
        private PickupType pickupType;
        private float fallSpeed;

        private void OnEnable()
        {
            EventManager.AddListener<LevelEvents.StageSpeedChangedEvent>(OnLevelSpeedChanged);
            InitializeFallSpeed(LevelModel.GetStageSpeed());
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<LevelEvents.StageSpeedChangedEvent>(OnLevelSpeedChanged);
        }

        private void Update()
        {
            MoveDownwards();
        }

        public void InitializeFallSpeed(float initialSpeed)
        {
            fallSpeed = initialSpeed;
        }

        public void UpdateFallSpeed(float newSpeed)
        {
            fallSpeed = newSpeed;
        }

        public void MoveDownwards()
        {
            transform.Translate(Vector3.down * (fallSpeed * Time.deltaTime));
        }

        private void OnLevelSpeedChanged(LevelEvents.StageSpeedChangedEvent evt)
        {
            UpdateFallSpeed(evt.StageSpeed);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            var evt = new PlayerEvents.PickupCollisionEvent(pickupType);
            EventManager.Broadcast(evt);
            Destroy(gameObject);
        }
    }
}