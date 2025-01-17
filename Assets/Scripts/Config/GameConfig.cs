using UnityEngine;

namespace Config
{
    public class GameConfig : MonoBehaviour
    {
        [SerializeField] private int startingStage = 1;
        [SerializeField] private float stageDuration = 20f;
        [SerializeField] private float speedIncreasePerStage = 2f;
        [SerializeField] private float baseStageSpeed = 5f;
        [SerializeField] private float baseStageWidth = 10f;
        [SerializeField] private float baseStageHeight = 5f;
        [SerializeField] private float baseObstacleSpawnInterval = 0.5f;
        [SerializeField] private float basePickupSpawnInterval = 5f;
        
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