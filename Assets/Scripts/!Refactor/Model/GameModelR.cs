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
    private int CurrentLevel { get; set; } = 1; 
    private const float secondsPerLevel = 5f;
    private static float elapsedTime { get; set; }
    public bool countDownActive;
    private int initialCountDownTime = 5;
    
    public GameState CurrentGameState
    {
        get => _currentGameState;
        set
        {
            if (_currentGameState == value) return;
            _currentGameState = value;
            EventManagerR.Broadcast(new GameEvents.GameStateChangedEventR(_currentGameState));
        }
    }
    
    public int GetCurrentLevel()
    {
        return CurrentLevel;
    }

    private void IncreaseLevel()
    {
        CurrentLevel++;
        EventManagerR.Broadcast(new LevelEvents.StageChangedEventR(CurrentLevel));
    }
    
    public static float GetElapsedTime() => elapsedTime;

    public void UpdateElapsedTime(float deltaTime)
    {
        elapsedTime += deltaTime;
        
        if (elapsedTime >= CurrentLevel * secondsPerLevel) IncreaseLevel();
    }
    
    public void ResetElapsedTime() => elapsedTime = 0f;
}