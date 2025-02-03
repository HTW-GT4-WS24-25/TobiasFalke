using Events;
using Utility;

namespace Model
{
    public class LevelModel
    {
        private int currentStage;
        private static float stageSpeed;
        private float stageWidth;
        private float stageHeight;
        private float obstacleSpawnInterval;
        private float pickupSpawnInterval;
        private static float elapsedTime;

        public int CurrentStage
        {
            get => currentStage;
            set
            {
                currentStage = value;
                EventManager.Trigger(new LevelEvent.StageChanged(CurrentStage));
            }
        }
        
        public float StageDuration { get; set; }
        
        public float StageSpeed
        {
            get => stageSpeed;
            set
            {
                stageSpeed = value;
                EventManager.Trigger(new LevelEvent.StageSpeedChanged(StageSpeed));
            }
        }
        public float StageWidth { get; set; }
        public float StageHeight { get; set; }
        public float ObstacleSpawnInterval { get; set; }
        public float PickupSpawnInterval { get; set; }
        
        public static float ElapsedTime
        {
            get => elapsedTime;
            set
            {
                elapsedTime = value;
                EventManager.Trigger(new LevelEvent.TimeElapsed(value));
            }
        }
    }
}