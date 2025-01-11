using System.Collections;
using UnityEngine;
using Utilities;

public class GameController : PersistentSingleton<GameController>
{
    private GameModel gameModel;
    [SerializeField]private GameView gameView;
    
    [SerializeField] private PlayerController _player;
    [SerializeField] private BackgroundScroller _backgroundScroller;
    
    private float remainingTime;

    private void Start()
    {
        gameModel = GameModel.Instance; // Access the singleton instance
        StartGame();

    }

    private void OnEnable()
    {
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<GameStartEvent>(OnStartGameEvent);
    }

    private void OnDestroy()
    {
        // Remove all event listeners when the player is destroyed.
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<GameStartEvent>(OnStartGameEvent);
        
    }
    
    void Update()
    {
        if (gameModel.IsPlaying)
        {
            gameModel.PlayTime += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
            return;
        }
        
        // Only proceed if the game is playing
        if (!gameModel.IsPlaying) return;

        // Game running logic
        gameModel.PlayTime += Time.deltaTime;
        int levelByTime = (int)(gameModel.PlayTime / gameModel.LevelDuration);
        if (levelByTime > gameModel.Level) ChangeLevel(levelByTime);
    }

    private void LateUpdate()
    {
        if (gameModel.IsCountingDown)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.unscaledDeltaTime; // Use unscaled time 
                gameView.UpdateCountDown(Mathf.Ceil(remainingTime));
            }

            if (remainingTime <= 0)
            {
                //TODO Fix Currently beging called right after GameOver because is player is being set to false 
                gameModel.IsCountingDown = false;
                StartGamePlay();
                Debug.Log("Started Game");
            }
        }
        // Update time counter UI if game is playing
        if (gameModel.IsPlaying)
        {
            gameView.timeCounter.text = Mathf.RoundToInt(gameModel.PlayTime).ToString();
        }
        
    }

    private void TogglePauseMenu()
    {
        if (gameModel.IsPlaying)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }
    
    public void ResumeGame()
    {
        gameView.SetPauseMenuUIVisibility(false);
        Time.timeScale = 1f; // Resume time
        gameModel.IsPlaying = false;
    }

    public void PauseGame()
    {
        gameView.SetPauseMenuUIVisibility(true);
        Time.timeScale = 0f; // Pause time
        gameModel.IsPlaying = false;
    }
    
    private void StartGame()
    {
        gameModel.IsCountingDown = true;
        remainingTime = gameModel.CountDownTime;
        gameView.ToggleCountDownVisibility(true);

        AudioManager.Instance.PlayTrack("mainSceneMusic");

        gameModel.IsPlaying = false;
        Time.timeScale = 0f; // Pause time
    }

    private void StartGamePlay()
    {
        gameModel.IsPlaying = true;
        remainingTime = 0;
        gameView.ToggleCountDownVisibility(false);
        Time.timeScale = 1f; // Unpause time
        AudioManager.Instance.PlaySound("gameStart");
    }
    
    private void OnStartGameEvent(GameStartEvent evt)
    {
        StartGame();
    }
    
    private void ChangeLevel(int newLevel)
    {
        gameModel.GameSpeed = 2.5f * newLevel + 10f;
        GameView.Instance.UpdateLevelCounter(newLevel);
        _backgroundScroller.UpdateLevelBackground(newLevel);
        gameModel.Level = newLevel;
    }
    
    private void OnGameOver(GameOverEvent evt)
    {     
        // Stop the game from playing
        GameModel.Instance.IsPlaying = false;
        Debug.Log("It's Game Over!");
        Time.timeScale = 0f;
        
        //TODO GameOver Sounds aren't playing only appears if started from Main Scene
        AudioManager.Instance.PlaySound("gameOver"); 
        // Start coroutine to delay scene change
        StartCoroutine(DelayedGameOver());
    }

    private IEnumerator DelayedGameOver()
    {
        // Wait for the length of the death animation (e.g., 2 seconds)
        yield return new WaitForSecondsRealtime(2f); 

        // Change the scene after delay
        SceneLoader.Instance.LoadScene(SceneLoader.GameOverScene);
        AudioManager.Instance.PlayTrack("gameOverMusic");
    }
}