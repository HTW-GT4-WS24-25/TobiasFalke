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

    void Start()
    {
        gameModel = GameModel.Instance; // Access the singleton instance
        gameModel.IsPlaying = false;
        remainingTime = gameModel.CountDownTime;
        
        Time.timeScale = 0f; // Pause time
        gameView.ToggleCountDownVisibility(true);
    }

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.unscaledDeltaTime; // Use unscaled time
            float remainingSeconds = Mathf.Ceil(remainingTime);
            gameView.UpdateCountDown(remainingSeconds);
        }
        if (remainingTime <= 0)
        {
            StartGame();
        }

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
        
        gameModel.IsPlaying = true;
        remainingTime = 0;
        gameView.ToggleCountDownVisibility(false);
        Time.timeScale = 1f; // Unpause time
        AudioManager.Instance.PlaySound("gameStart");
    }
    
    private void ChangeLevel(int newLevel)
    {
        gameModel.GameSpeed = 2.5f * newLevel + 10f;
        GameView.Instance.UpdateLevelCounter(newLevel);
        _backgroundScroller.UpdateLevelBackground(newLevel);
        gameModel.Level = newLevel;
    }
    
        
    private void OnGameOver()
    {     
        // Stop the game from playing
        GameModel.Instance.IsPlaying = false;
        Time.timeScale = 0f;
        
        AudioManager.Instance.PlaySound("gameOver"); 
        // Start coroutine to delay scene change
        StartCoroutine(DelayedGameOver());
    }

    private IEnumerator DelayedGameOver()
    {
        // Wait for the length of the death animation (e.g., 2 seconds)
        yield return new WaitForSecondsRealtime(2f); 

        // Change the scene after delay
        SceneLoader.Instance.LoadScene(SceneLoader.gameOver);
        AudioManager.Instance.PlayTrack("gameOverMusic");
    }
}