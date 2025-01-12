
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
    
    public bool startCountDownActive;
    public int startCountdownTime = 5;
    public static float ElapsedTime { get; private set; }
    public int CurrentLevel { get; private set; } = 1; 
    private const float levelTimeThreshold = 5f;


    public GameState CurrentGameState
    {
        get => _currentGameState;
        set
        {
            if (_currentGameState == value) return;
            _currentGameState = value;
            EventManagerR.Broadcast(new GameEvents.GameStateChangedEvent(_currentGameState));
        }
    }
    
    public static float GetElapsedTime() => ElapsedTime;

    public void UpdateElapsedTime(float deltaTime)
    {
        ElapsedTime += deltaTime;
        
        if (ElapsedTime >= CurrentLevel * levelTimeThreshold)
        {
            IncreaseLevel();
        }
    }
    
    public void ResetElapsedTime() => ElapsedTime = 0f;
    
    public int GetCurrentLevel()
    {
        return CurrentLevel;
    }

    private void IncreaseLevel()
    {
        CurrentLevel++;
        EventManagerR.Broadcast(new GameEvents.LevelChangedEvent(CurrentLevel));
    }
}