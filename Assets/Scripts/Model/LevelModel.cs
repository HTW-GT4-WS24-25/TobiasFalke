using Events;

namespace Model
{
    public class LevelModel
    {
        private int currentStage = 1;
        private float obstacleSpawnInterval = 0.1f;
        private float pickUpSpawnInterval = 5.0f;
        private static float stageSpeed = 5.0f;
        private float stageWidth = 10.0f;
        private static float elapsedTime;

        public int GetCurrentStage() => currentStage;
        public void SetCurrentStage(int newStage) => currentStage = newStage;

        public float GetStageWidth() => stageWidth;
        public void SetStageWidth(float width) => stageWidth = width;

        public float GetObstacleSpawnInterval() => obstacleSpawnInterval;
        public void SetObstacleSpawnInterval(float interval) => obstacleSpawnInterval = interval;

        public float GetPickUpSpawnInterval() => pickUpSpawnInterval;
        public void SetPickUpSpawnInterval(float interval) => pickUpSpawnInterval = interval;

        public static float GetStageSpeed() => stageSpeed;
        public static void SetStageSpeed(float speed) => stageSpeed = speed;
        
        public float ElapsedTime
        {
            get => elapsedTime;
            set
            {
                elapsedTime = value;
                EventManager.Broadcast(new LevelEvents.TimeElapsed(value));
            }
        }
    }
}