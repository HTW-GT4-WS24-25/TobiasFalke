using UnityEngine;

public class GameEvents
{
    public class GameStateChangedEvent : GameEvent
    {
        public GameModelR.GameState NewGameState { get; }

        public GameStateChangedEvent(GameModelR.GameState newGameState)
        {
            NewGameState = newGameState;
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

    public class ToggleCountDownEvent : GameEvent
    {

        public bool SetActive { get; }

        public ToggleCountDownEvent(bool setActive)
        {
            SetActive = setActive;
        }
    }
}