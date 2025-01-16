using UnityEngine;
using UnityEngine.Serialization;

namespace Utility
{
    public class GameConfig : MonoBehaviour
    {
        public int startingStage = 1;
        public float stageDuration = 20f;
        public float speedIncreasePerStage = 2f;
        public float baseStageSpeed = 5f;
        public float baseStageWidth = 10f;
        public float baseObstacleSpawnInterval = 0.5f;
        public float basePickupSpawnInterval = 5f;
        
        public static int StartingStage;
        public static float StageDuration;
        public static float SpeedIncreasePerStage;
        public static float BaseStageSpeed;
        public static float BaseStageWidth;
        public static float BaseObstacleSpawnInterval;
        public static float BasePickupSpawnInterval;
        
        private void Awake()
        {
            StartingStage = startingStage;
            StageDuration = stageDuration;
            SpeedIncreasePerStage = speedIncreasePerStage;
            BaseStageSpeed = baseStageSpeed;
            BaseStageWidth = baseStageWidth;
            BaseObstacleSpawnInterval = baseObstacleSpawnInterval;
            BasePickupSpawnInterval = basePickupSpawnInterval;
        }
    }
}