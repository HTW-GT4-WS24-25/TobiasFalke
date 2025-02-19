using Utility;

namespace Events
{
    public static class LevelEvent
    {
        public class StageChanged : Event
        {
            public int NewStage { get; }

            public StageChanged(int newStage)
            {
                NewStage = newStage;
            }
        }

        public class StageSpeedChanged : Event
        {
            public float StageSpeed { get; private set; }

            public StageSpeedChanged(float stageSpeed)
            {
                StageSpeed = stageSpeed;
            }
        }
        
        public class TimeElapsed : Event
        {
            public float NewTime { get; private set; }

            public TimeElapsed(float newTime)
            {
                NewTime += newTime;
            }
        }
        
        public class ToggleCountDown : Event
        {
            public bool SetActive { get; }

            public ToggleCountDown(bool setActive)
            {
                SetActive = setActive;
            }
        }

        public class TogglePauseMenu : Event
        {
            public bool IsPaused { get; }

            public TogglePauseMenu(bool isPaused)
            {
                IsPaused = isPaused;
            }
        }
    }
}