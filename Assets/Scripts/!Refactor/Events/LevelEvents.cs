public static class LevelEvents
{
    public class StageChangedEventR : GameEvent
    {
        public int NewStage { get; }

        public StageChangedEventR(int newStage)
        {
            NewStage = newStage;
        }
    }

    public class StageSpeedChangedEventR : GameEvent
    {
        public float StageSpeed { get; private set; }

        public StageSpeedChangedEventR(float stageSpeed)
        {
            StageSpeed = stageSpeed;
        }
    }
}