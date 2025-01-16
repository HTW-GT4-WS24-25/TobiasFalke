namespace Events
{
    public static class LevelEvents
    {
        public class StageChanged : GameEvent
        {
            public int NewStage { get; }

            public StageChanged(int newStage)
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
        
        public class TimeElapsed : GameEvent
        {
            public float NewTime { get; private set; }

            public TimeElapsed(float newTime)
            {
                NewTime += newTime;
            }
        }
        
        public class ToggleCountDown : GameEvent
        {
            public bool SetActive { get; }

            public ToggleCountDown(bool setActive)
            {
                SetActive = setActive;
            }
        }

        public class TogglePauseMenu : GameEvent
        {
            public bool IsPaused { get; }

            public TogglePauseMenu(bool isPaused)
            {
                IsPaused = isPaused;
            }
        }
    }
}