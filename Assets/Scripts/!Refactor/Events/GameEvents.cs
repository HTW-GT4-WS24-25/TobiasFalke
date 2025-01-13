using UnityEngine;

public static class GameEvents
{
    public class GameStateChangedEvent : GameEvent
    {
        public GameModelR.GameState NewGameState { get; }

        public GameStateChangedEvent(GameModelR.GameState newGameState)
        {
            NewGameState = newGameState;
        }
    }

    public class ToggleCountDownEvent : GameEvent
    {
        public bool SetActive { get; }

        public ToggleCountDownEvent(bool setActive)
        {
            SetActive = setActive;
        }
    }

    public class TogglePauseMenuEvent : GameEvent
    {
        public bool IsPaused { get; }

        public TogglePauseMenuEvent(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }
    
    public class LevelChangedEvent : GameEvent
    {
        public int NewLevel { get; }

        public LevelChangedEvent(int newLevel)
        {
            NewLevel = newLevel;
        }
    }
}