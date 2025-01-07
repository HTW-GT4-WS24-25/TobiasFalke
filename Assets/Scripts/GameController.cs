using UnityEngine;
using Utilities;

public class GameController : PersistentSingleton<GameController>
{
    private GameModel gameModel;
    [SerializeField]private GameView gameView;
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
            remainingTime -= Time.unscaledDeltaTime;
            float remainingSeconds = remainingTime % 60;
            gameView.UpdateCountDown(remainingSeconds);
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
            gameModel.IsPlaying = true;
            gameView.ToggleCountDownVisibility(false);
            Time.timeScale = 1f; // Unpause time
            AudioManager.Instance.PlaySound("openSettings");
        }

        if (gameModel.IsPlaying)
        {
            gameModel.PlayTime += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }
    }

    void LateUpdate()
    {
        gameView.UpdateTimeCounter(UpdateTimer().ToString());
    }

    public void TriggerGameOver()
    {
        gameModel.IsPlaying = false;
    }

    private void TogglePauseGame()
    {
        if (gameModel.IsPlaying)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Resume()
    {
        gameView.SetPauseMenuUIVisibility(false);
        Time.timeScale = 1f; // Unpause time
        gameModel.IsPlaying = true;
    }

    public void Pause()
    {
        gameView.SetPauseMenuUIVisibility(true);
        Time.timeScale = 0f; // Pause time
        gameModel.IsPlaying = false;
    }

    private int UpdateTimer()
    {
        return Mathf.RoundToInt(gameModel.PlayTime);
    }
}