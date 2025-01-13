using UnityEngine;

public static class LevelEvents
{
    public class StageChangedEvent : GameEvent
    {
        public int NewStage { get; }

        public StageChangedEvent(int newStage)
        {
            NewStage = newStage;
        }
    }

    public class StageSpeedChangedEvent : GameEvent
    {
        public float StageSpeed { get; private set; }

        public StageSpeedChangedEvent(float stageSpeed)
        {
            StageSpeed = stageSpeed;
        }
    }
}