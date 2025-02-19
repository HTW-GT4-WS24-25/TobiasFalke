using UnityEngine;

namespace Utility
{
    public class GameConfig : MonoBehaviour
    {
        public static GameConfig Instance { get; set; }
        
        /*
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
*/

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

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
            /*
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
        */
        }
    }
}