public static class GameEvents
{
    public class GameStateChangedEventR : GameEvent
    {
        public GameModelR.GameState NewGameState { get; }

        public GameStateChangedEventR(GameModelR.GameState newGameState)
        {
            NewGameState = newGameState;
        }
    }

    public class ToggleCountDownEventR : GameEvent
    {
        public bool SetActive { get; }

        public ToggleCountDownEventR(bool setActive)
        {
            SetActive = setActive;
        }
    }

    public class TogglePauseMenuEventR : GameEvent
    {
        public bool IsPaused { get; }

        public TogglePauseMenuEventR(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }
    
    public class LevelChangedEventR : GameEvent
    {
        public int NewLevel { get; }

        public LevelChangedEventR(int newLevel)
        {
            NewLevel = newLevel;
        }
    }
}