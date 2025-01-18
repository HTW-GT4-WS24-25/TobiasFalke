using UnityEngine;

namespace Utility
{
    public class GameConfig : MonoBehaviour
    {
        [Header("Level Settings")]
        [SerializeField] private int startingStage = 1;
        [SerializeField] private float stageDuration = 20f;
        [SerializeField] private float speedIncreasePerStage = 2f;
        [SerializeField] private float baseStageSpeed = 5f;
        [SerializeField] private float baseStageWidth = 10f;
        [SerializeField] private float baseStageHeight = 5f;
        [SerializeField] private float baseObstacleSpawnInterval = 0.5f;
        [SerializeField] private float basePickupSpawnInterval = 5f;
        [Header("Player Settings")]
        [SerializeField] private int maxHealthPoints = 100;
        [SerializeField] private int maxSpecialPoints = 100;
        [SerializeField] private float baseSpeed = 5f;
        [SerializeField] private float maxSpeedMultiplier = 3f;
        [SerializeField] private float baseJumpHeight = 2f;
        [SerializeField] private float baseJumpDuration = 1f;
        [SerializeField] private float baseGrindActionScore = 1f;
        [SerializeField] private float trickActionScore = 1f;
        [SerializeField] private float trickActionDuration = 1f;
        [SerializeField] private float specialActionDuration = 8f;
        [SerializeField] private float invincibilityDuration = 1f;

        public static int MaxHealthPoints;
        public static int MaxSpecialPoints;
        public static float BaseSpeed;
        public static float MaxSpeedMultiplier;
        public static float JumpHeight;
        public static float JumpDuration;
        public static float GrindActionScore;
        public static float TrickActionScore;
        public static float TrickActionDuration;
        public static float SpecialActionDuration;
        public static float InvincibilityDuration;
        public static int StartingStage;
        public static float StageDuration;
        public static float SpeedIncreasePerStage;
        public static float BaseStageSpeed;
        public static float BaseStageWidth;
        public static float BaseStageHeight;
        public static float BaseObstacleSpawnInterval;
        public static float BasePickupSpawnInterval;

        private void Awake()
        {
            MaxHealthPoints = maxHealthPoints;
            MaxSpecialPoints = maxSpecialPoints;
            MaxSpeedMultiplier = maxSpeedMultiplier;
            BaseSpeed = baseSpeed;
            JumpHeight = baseJumpHeight;
            JumpDuration = baseJumpDuration;
            GrindActionScore = baseGrindActionScore;
            TrickActionScore = trickActionScore;
            TrickActionDuration = trickActionDuration;
            SpecialActionDuration = specialActionDuration;
            InvincibilityDuration = invincibilityDuration;
            StartingStage = startingStage;
            StageDuration = stageDuration;
            SpeedIncreasePerStage = speedIncreasePerStage;
            BaseStageSpeed = baseStageSpeed;
            BaseStageWidth = baseStageWidth;
            BaseStageHeight = baseStageHeight;
            BaseObstacleSpawnInterval = baseObstacleSpawnInterval;
            BasePickupSpawnInterval = basePickupSpawnInterval;
        }
    }
}