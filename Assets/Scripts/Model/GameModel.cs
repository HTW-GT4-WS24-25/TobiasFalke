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
                if (currentGameState == value) return;
                currentGameState = value;
                EventManager.Broadcast(new StateChanged(currentGameState));
            }
        }
        
        public class StateChanged : Event
        {
            public GameState NewGameState { get; }

            public StateChanged(GameState newGameState)
            {
                NewGameState = newGameState;
            }
        }
    }
}