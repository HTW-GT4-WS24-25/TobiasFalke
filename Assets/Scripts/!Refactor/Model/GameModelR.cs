
public class GameModelR
{
    public enum GameState
    {
        Menu,
        Running,
        Paused,
        GameOver
    }
    
    private GameState _currentGameState;
    
    // Delegates and events for state changes;
    public delegate void GameStateChangedHandler(GameState newGameState);
    public event GameStateChangedHandler OnGameStateChanged;

    public bool startCountDownActive;
    public int startCountdownTime = 5;

    public GameState CurrentGameState
    {
        get => _currentGameState;
        set
        {
            if (_currentGameState == value) return;
            _currentGameState = value;
            // Broadcast the state change event
            EventManagerR.Broadcast(new GameEvents.GameStateChangedEvent(_currentGameState));
        }
    }
}