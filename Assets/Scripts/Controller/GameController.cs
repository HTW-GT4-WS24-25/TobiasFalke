using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    private GameModel gameModel;
    private InputManager inputManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            gameModel = new GameModel();
            inputManager = InputManager.Instance;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameModel.CurrentGameState = GameModel.GameState.Running; // Start the game in the running state for development
        EventManager.AddListener<GameEvents.GameStateChangedEventR>(OnGameStateChanged);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameEvents.GameStateChangedEventR>(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameEvents.GameStateChangedEventR obj)
    {
        if (obj.NewGameState == GameModel.GameState.GameOver)
        {
            SceneLoader.Instance.LoadScene(SceneLoader.gameOver);
            AudioManager.Instance.PlayTrack("gameOverMusic");
        }
        else if (obj.NewGameState == GameModel.GameState.Running)
        {
            AudioManager.Instance.PlayTrack("mainSceneMusic");
        }
    }

    void Update()
    {
        if (inputManager.GetPauseInput())
        {
            TogglePause();
        }
        if (gameModel.CurrentGameState == GameModel.GameState.Running)
        {
            gameModel.UpdateElapsedTime(Time.deltaTime);
        }
    }

    public void StartGame()
    {
        Debug.Log("Game has been started.");
        gameModel.CurrentGameState = GameModel.GameState.Running;
        gameModel.countDownActive = true;
        Time.timeScale = 0f; 
        gameModel.ResetElapsedTime();
    }

    private void TogglePause()
    {
        if (gameModel.CurrentGameState == GameModel.GameState.Running) PauseGame();
        else if (gameModel.CurrentGameState == GameModel.GameState.Paused) ResumeGame();
    }

    public void PauseGame()
    {
        gameModel.CurrentGameState = GameModel.GameState.Paused;
        Time.timeScale = 0f; // Pause game physics and updates
        EventManager.Broadcast(new GameEvents.TogglePauseMenuEventR(true));
    }

    public void ResumeGame()
    {
        gameModel.CurrentGameState = GameModel.GameState.Running;
        Time.timeScale = 1f; // Resume game physics and updates
        EventManager.Broadcast(new GameEvents.TogglePauseMenuEventR(false));
    }
}