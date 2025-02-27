using Utility;

namespace Model
{
    public class GameModel
    {
        public enum GameState
        {
            Menu,
            Running,
            Paused,
            Loose,
            Quit
        }

        private GameState currentGameState;
        
        public GameState CurrentGameState
        {
            get => currentGameState;
            set
            {
                currentGameState = value;
                EventManager.Trigger(new GameStateChanged(currentGameState));
            }
        }
        
        public class GameStateChanged : Event
        {
            public GameState NewGameState { get; }

            public GameStateChanged(GameState newGameState)
            {
                NewGameState = newGameState;
            }
        }
    }
}