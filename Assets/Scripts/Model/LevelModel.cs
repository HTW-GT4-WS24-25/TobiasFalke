using Events;

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
                EventManager.Broadcast(new LevelEvent.StageChanged(currentStage));
            }
        }
        
        public float StageDuration { get; set; }
        
        public float StageSpeed
        {
            get => stageSpeed;
            set
            {
                stageSpeed = value;
                EventManager.Broadcast(new LevelEvent.StageSpeedChanged(stageSpeed));
            }
        }
        public float StageWidth { get; set; }
        
        public float StageHeight { get; set; }
        public float ObstacleSpawnInterval { get; set; }
        public float PickupSpawnInterval { get; set; }
        
        public float ElapsedTime
        {
            get => elapsedTime;
            set
            {
                elapsedTime = value;
                EventManager.Broadcast(new LevelEvent.TimeElapsed(value));
            }
        }
        
    }
}