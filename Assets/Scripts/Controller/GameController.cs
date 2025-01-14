using Events;
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
        gameModel.CurrentGameState = GameModel.GameState.Running;
        EventManager.AddListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameEvents.GameStateChangedEvent evt)
    {
        switch (evt.NewGameState)
        {
            case GameModel.GameState.Running:
                AudioManager.Instance.PlayTrack("mainSceneMusic");
                break;
            case GameModel.GameState.Paused:
                break;
            case GameModel.GameState.Menu:
                break;
            case GameModel.GameState.GameOver:
                SceneLoader.Instance.LoadScene(SceneLoader.gameOver);
                AudioManager.Instance.PlayTrack("gameOverMusic");
                break;
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
        gameModel.ResetElapsedTime();
    }

    private void TogglePause()
    {
        if (gameModel.CurrentGameState == GameModel.GameState.Running)
            PauseGame();
        else if (gameModel.CurrentGameState == GameModel.GameState.Paused)
            ResumeGame();
    }

    public void PauseGame()
    {
        gameModel.CurrentGameState = GameModel.GameState.Paused;
        Time.timeScale = 0f;
        EventManager.Broadcast(new GameEvents.TogglePauseMenuEvent(true));
    }

    public void ResumeGame()
    {
        gameModel.CurrentGameState = GameModel.GameState.Running;
        Time.timeScale = 1f;
        EventManager.Broadcast(new GameEvents.TogglePauseMenuEvent(false));
    }
}