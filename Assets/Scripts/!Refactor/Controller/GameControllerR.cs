using UnityEngine;

public class GameControllerR : MonoBehaviour
{
    public static GameControllerR Instance { get; private set; }
    private GameModelR gameModel;
    private InputManager inputManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            gameModel = new GameModelR();
            inputManager = InputManager.Instance;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameModel.CurrentGameState = GameModelR.GameState.Running; // Start the game in the running state for development
    }

    void Update()
    {
        if (inputManager.GetPauseInput())
        {
            TogglePause();
        }
        if (gameModel.CurrentGameState == GameModelR.GameState.Running)
        {
            gameModel.UpdateElapsedTime(Time.deltaTime);
        }
    }

    public void StartGame()
    {
        Debug.Log("Game has been started.");
        gameModel.CurrentGameState = GameModelR.GameState.Running;
        gameModel.countDownActive = true;
        Time.timeScale = 0f; 
        gameModel.ResetElapsedTime();
    }

    private void TogglePause()
    {
        if (gameModel.CurrentGameState == GameModelR.GameState.Running) PauseGame();
        else if (gameModel.CurrentGameState == GameModelR.GameState.Paused) ResumeGame();
    }

    public void PauseGame()
    {
        gameModel.CurrentGameState = GameModelR.GameState.Paused;
        Time.timeScale = 0f; // Pause game physics and updates
        EventManagerR.Broadcast(new GameEvents.TogglePauseMenuEvent(true));
    }

    public void ResumeGame()
    {
        gameModel.CurrentGameState = GameModelR.GameState.Running;
        Time.timeScale = 1f; // Resume game physics and updates
        EventManagerR.Broadcast(new GameEvents.TogglePauseMenuEvent(false));
    }
    
    public void GameOver()
    {
        gameModel.CurrentGameState = GameModelR.GameState.GameOver;
    }
}