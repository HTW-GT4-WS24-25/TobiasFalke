using Events;

public class GameModel
{
    public enum GameState
    {
        Menu,
        Running,
        Paused,
        GameOver
    }

    private GameState _currentGameState;
    private int currentLevel = 1; 
    private const float SecondsPerLevel = 5f;
    private static float elapsedTime;

    public GameState CurrentGameState
    {
        get => _currentGameState;
        set
        {
            if (_currentGameState == value) return;
            _currentGameState = value;
            EventManager.Broadcast(new GameEvents.GameStateChangedEvent(_currentGameState));
        }
    }

    public int GetCurrentLevel() => currentLevel;

    private void IncreaseLevel()
    {
        currentLevel++;
        EventManager.Broadcast(new LevelEvents.StageChangedEvent(currentLevel));
    }

    public static float GetElapsedTime() => elapsedTime;

    public void UpdateElapsedTime(float deltaTime)
    {
        elapsedTime += deltaTime;
        if (elapsedTime >= currentLevel * SecondsPerLevel)
            IncreaseLevel();
    }

    public void ResetElapsedTime() => elapsedTime = 0f;
}